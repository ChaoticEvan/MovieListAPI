using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Text.Json;

namespace MovieListAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieListController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MovieListController> _logger;
        private IHttpClientFactory _httpClientFactory;

        private const string API_URL = "https://api-gate2.movieglu.com/";
        private const string API_CLIENT_NAME = "NCIN";
        private const string DATE_HEADER_POSTFIX = ".360Z";


        public MovieListController(ILogger<MovieListController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet(Name = "GetMovieList")]
        public async Task<IEnumerable<Movie>> Get()
        {
            List<Movie> movies = new List<Movie>();
            JsonElement nowPlayingContent = await GetNowPlaying();

            foreach (var requestMovie in nowPlayingContent.GetProperty("films").EnumerateArray())
            {
                Movie movie = new Movie()
                {
                    Id = requestMovie.GetProperty("film_id").GetInt32(),
                    Title = requestMovie.GetProperty("film_name").ToString(),
                    TrailerLink = requestMovie.GetProperty("film_trailer").ToString(),
                    ReleaseDate = DateOnly.Parse(requestMovie.GetProperty("release_dates")[0].GetProperty("release_date").ToString()),
                    PosterLink = requestMovie.GetProperty("images").GetProperty("poster").GetProperty("1").GetProperty("medium").GetProperty("film_image").ToString()
                };
                movies.Add(movie);
            }

            return movies;
        }

        /// <summary>
        /// Gets a list of movies now playing.
        /// 
        /// Sends request to filmsNowShowing at MovieGlue
        /// </summary>
        /// <returns>JSON Response from HTTP Request to filmsNowShowing</returns>
        private async Task<dynamic> GetNowPlaying()
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
                    { "device-datetime", DateTime.Now.ToString("s") + DATE_HEADER_POSTFIX }
                }
            };

            var httpClient = _httpClientFactory.CreateClient();
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
        /// Sends request to filmShowTimes at MovieGlue
        /// </summary>
        /// <param name="filmId">Internal MovieGlu ID to get list of showings in their DB</param>
        /// <returns>JSON Response from HTTP Request to filmShowTimes</returns>
        private async Task<dynamic> GetShowings(int filmId)
        {
            var getTheatersRequest = new HttpRequestMessage(
                HttpMethod.Get,
                API_URL + "filmShowTimes/?film_id=" + filmId.ToString() + "&date=" + DateOnly.FromDateTime(DateTime.Now).ToString())
            {
                Headers =
                {
                    { "client", API_CLIENT_NAME },
                    { "x-api-key", _configuration["MovieListAPI:MovieGluApiKey"]},
                    { "authorization", _configuration["MovieListAPI:MovieGluAuth"] },
                    { "territory", "XX" },
                    { "api-version", "v200" },
                    { "geolocation", "-22.0;14.0" },
                    { "device-datetime", DateTime.Now.ToString("s") + DATE_HEADER_POSTFIX }
                }
            };
            return null;
        }
    }
}