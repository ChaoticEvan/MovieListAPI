using System.Text.Json;

namespace MovieListAPI
{
    public class ResponseHandler
    {
        private IHttpClientFactory _httpClientFactory;
        private IConfiguration _configuration;
        private RequestHandler _requestHandler;
        public ResponseHandler(IHttpClientFactory httpClientFactory, IConfiguration configuration) 
        { 
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _requestHandler = new RequestHandler(httpClientFactory, configuration);            
        }
        public async Task<IEnumerable<Movie>> GetResponse()
        {
            List<Movie> movies = new List<Movie>();
            JsonElement nowPlayingContent = await _requestHandler.GetNowPlaying();

            foreach (var requestMovie in nowPlayingContent.GetProperty("films").EnumerateArray())
            {
                movies.Add(await BuildMovie(requestMovie));
            }
            return movies;
        }

        private async Task<Movie> BuildMovie(JsonElement jsonMovie)
        {
            Movie movie = new Movie()
            {
                Id = jsonMovie.GetProperty("film_id").GetInt32(),
                Title = jsonMovie.GetProperty("film_name").ToString(),
                TrailerLink = jsonMovie.GetProperty("film_trailer").ToString(),
                ReleaseDate = DateOnly.Parse(jsonMovie.GetProperty("release_dates")[0].GetProperty("release_date").ToString()),
                PosterLink = jsonMovie.GetProperty("images").GetProperty("poster").GetProperty("1").GetProperty("medium").GetProperty("film_image").ToString()
            };

            JsonElement showingsContent = await _requestHandler.GetShowings(movie.Id);

            return movie;
        }
    }
}
