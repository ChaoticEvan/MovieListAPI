using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

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


        public MovieListController(ILogger<MovieListController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet(Name = "GetMovieList")]
        public async Task<IEnumerable<Movie>> Get()
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
                    { "terriroty", "XX" },
                    { "api-version", "v200" },
                    { "geolocation", "-22.0;14.0" },
                    { "device-datetime", DateTime.Now.ToString("s") }
                }
            };

            var httpClient = _httpClientFactory.CreateClient();
            var nowShowingMoviesResponse = await httpClient.SendAsync(nowShowingMoviesRequest);
            return new List<Movie>();
        }
    }
}