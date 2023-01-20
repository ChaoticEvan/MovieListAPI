using Microsoft.AspNetCore.Mvc;

namespace MovieListAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieListController : ControllerBase
    {
        private readonly ILogger<MovieListController> _logger;

        public MovieListController(ILogger<MovieListController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetMovieList")]
        public IEnumerable<Movie> Get()
        {
            return new List<Movie>();
        }
    }
}