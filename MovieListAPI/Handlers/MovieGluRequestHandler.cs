using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text.Json;

namespace MovieListAPI.Handlers
{
    /// <summary>
    /// Class for handling HTTP requests to MovieGlu API
    /// </summary>
    public class MovieGluRequestHandler
    {
        private IHttpClientFactory _httpClientFactory;
        private IConfiguration _configuration;

        // Constants
        private const string API_URL = "https://api-gate2.movieglu.com/";
        private const string API_CLIENT_NAME = "NCIN";
        private const string MOVIEGLU_DATE_HEADER_SUFFIX = ".360Z";
        private const string MOVIEGLU_NO_CONTENT_HEADER_MESSAGE = "No results for request";
        /// <summary>
        /// Constructs a MovieGluRequestHandler object for handling requests sent to the MovieGlu API
        /// </summary>
        /// <param name="httpClientFactory">Factory used to create HTTP client for sending requests</param>
        /// <param name="configuration">Configuration that contains client secrets</param>
        public MovieGluRequestHandler(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        /// <summary>
        /// Gets a list of movies now playing.
        /// 
        /// Sends request to filmsNowShowing at MovieGlue.
        /// </summary>
        /// <returns>JSON Response from HTTP Request to filmsNowShowing</returns>
        public async Task<dynamic> GetNowPlaying()
        {
            var nowShowingMoviesRequest = new HttpRequestMessage(
                HttpMethod.Get,
                API_URL + "filmsNowShowing")
            {
                Headers =
                {
                    { "client", API_CLIENT_NAME },
                    { "x-api-key", _configuration["MovieListAPI:MovieGluApiKey"]},
                    { "authorization", _configuration["MovieListAPI:MovieGluAuth"] },
                    { "territory", "XX" },
                    { "api-version", "v200" },
                    { "geolocation", "-22.0;14.0" },
                    { "device-datetime", DateTime.Now.ToString("s") + MOVIEGLU_DATE_HEADER_SUFFIX }
                }
            };

            var httpClient = CreateClient(_httpClientFactory);
            var nowShowingMoviesResponse = await httpClient.SendAsync(nowShowingMoviesRequest);

            if (!nowShowingMoviesResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Unsuccessful filmsNowShowing MovieGlu request");
            }

            using (var stream = await nowShowingMoviesResponse.Content.ReadAsStreamAsync())
            {
                return JsonSerializer.Deserialize<dynamic>(stream);
            }
        }

        /// <summary>
        /// Gets list of showings for given movie.
        /// 
        /// Sends request to filmShowTimes at MovieGlue.
        /// </summary>
        /// <param name="filmId">Internal MovieGlu ID to get list of showings in their DB</param>
        /// <returns>JSON Response from HTTP Request to filmShowTimes</returns>
        public async Task<dynamic> GetShowings(int filmId)
        {
            var getTheatersRequest = new HttpRequestMessage(
                HttpMethod.Get,
                API_URL + "filmShowTimes/?film_id=" + filmId.ToString() + "&date=" + DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd"))
            {
                Headers =
                {
                    { "client", API_CLIENT_NAME },
                    { "x-api-key", _configuration["MovieListAPI:MovieGluApiKey"]},
                    { "authorization", _configuration["MovieListAPI:MovieGluAuth"] },
                    { "territory", "XX" },
                    { "api-version", "v200" },
                    { "geolocation", "-22.0;14.0" }, // TODO: Covert ZIP to long/lat
                    { "device-datetime", DateTime.Now.ToString("s") + MOVIEGLU_DATE_HEADER_SUFFIX }
                }
            };

            var httpClient = CreateClient(_httpClientFactory);
            var getTheatersResponse = await httpClient.SendAsync(getTheatersRequest);

            if (!getTheatersResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Unsuccessful filmShowTimes MovieGlu request");
            }

            // TODO: Test this
            if (!AreTheatersShowingThisMovie(getTheatersResponse))
            {
                throw new NoContentException("No theaters are showing this movie");
            }

            using (var stream = await getTheatersResponse.Content.ReadAsStreamAsync())
            {
                return JsonSerializer.Deserialize<dynamic>(stream);
            }
        }

        /// <summary>
        /// Helper method for creating an HTTP CLient
        /// </summary>
        /// <param name="httpClientFactory">Factory used to create HTTP Client</param>
        /// <returns>The new created client from the given factory</returns>
        private HttpClient CreateClient(IHttpClientFactory httpClientFactory)
        {
            return httpClientFactory.CreateClient();
        }

        /// <summary>
        /// Helper method to determine if response from MovieGlu was a NoContent and no theaters nearby are showing this movie
        /// </summary>
        /// <param name="getTheatersResponse">Response from MovieGlu</param>
        /// <returns>True if receive a No Content response and the appropriate message from MovieGlu. False otherwise</returns>
        private bool AreTheatersShowingThisMovie(HttpResponseMessage getTheatersResponse)
        {
            IEnumerable<string>? messageHeaders;
            bool ifReceivedNoContentResponse = getTheatersResponse.StatusCode == System.Net.HttpStatusCode.NoContent;
            if (!ifReceivedNoContentResponse)
            {
                return true;
            }

            bool ifResponseHeadersContainsMessage = getTheatersResponse.Headers.TryGetValues("MG-message", out messageHeaders);
            bool ifNoTheatersAreShowingMovie = false;
            if (ifResponseHeadersContainsMessage)
            {
                ifNoTheatersAreShowingMovie = messageHeaders.First().Equals(MOVIEGLU_NO_CONTENT_HEADER_MESSAGE);
            }

            return !(ifReceivedNoContentResponse && ifNoTheatersAreShowingMovie);
        }
    }

    /// <summary>
    /// Custom exception class for receiving a No Content response from MovieGlu
    /// </summary>
    [Serializable]
    internal class NoContentException : Exception
    {
        public NoContentException()
        {
        }

        public NoContentException(string? message) : base(message)
        {
        }
    }
}
