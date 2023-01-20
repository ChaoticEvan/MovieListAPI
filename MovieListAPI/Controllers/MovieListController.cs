using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace MovieListAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieListController : ControllerBase
    {
        private readonly ILogger<MovieListController> _logger;

        private IHttpClientFactory _httpClientFactory;

        private const string COUNTRY_CODE = "US";

        private const string LANGUAGE_CODE = "en";

        public MovieListController(ILogger<MovieListController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet(Name = "GetMovieList")]
        public async Task<IEnumerable<Movie>> Get()
        {
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                "https://api.themoviedb.org/3/movie/now_playing?")
            {
                Headers =
                {
                    { HeaderNames.Accept, "application/vnd.github.v3+json" },
                    { HeaderNames.UserAgent, "HttpRequestsSample" }
                }
            };

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(httpRequestMessage);
            return new List<Movie>();
        }
    }
}