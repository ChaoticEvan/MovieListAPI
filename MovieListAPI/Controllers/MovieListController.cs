using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Net.Http;
using System.Runtime.InteropServices;
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

        public MovieListController(ILogger<MovieListController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet(Name = "GetMovieList")]
        public async Task<IEnumerable<Movie>> Get()
        {
            ResponseHandler responseHandler = new ResponseHandler(_httpClientFactory, _configuration);
            return await responseHandler.GetResponse();
        }
    }
}