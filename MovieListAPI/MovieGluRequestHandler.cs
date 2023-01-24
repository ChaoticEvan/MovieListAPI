using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MovieListAPI
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
        
        // This is necessary for the timestamp header for MovieGlu.
        // Not sure why.
        private const string DATE_HEADER_SUFFIX = ".360Z";

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
                    { "device-datetime", DateTime.Now.ToString("s") + DATE_HEADER_SUFFIX }
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
                    { "geolocation", "-22.0;14.0" },
                    { "device-datetime", DateTime.Now.ToString("s") + DATE_HEADER_SUFFIX }
                }
            };

            var httpClient = CreateClient(_httpClientFactory);
            var getTheatersResponse = await httpClient.SendAsync(getTheatersRequest);

            if (!getTheatersResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Unsuccessful filmShowTimes MovieGlu request");
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
        private static HttpClient CreateClient(IHttpClientFactory httpClientFactory)
        {
            return httpClientFactory.CreateClient();
        }
    }
}
