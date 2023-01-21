using System.Text.Json;

namespace MovieListAPI
{
    /// <summary>
    /// Creates a response to return to MovieListAPI end-user
    /// </summary>
    public class ResponseHandler
    {
        /// <summary>
        /// Object for sending requests to Movie Glu API
        /// </summary>
        private MovieGluRequestHandler _movieGlueRequestHandler;

        /// <summary>
        /// Constructs a ResponseHandler object
        /// </summary>
        /// <param name="httpClientFactory">Factory used to create HTTP client for sending requests</param>
        /// <param name="configuration">Configuration that contains client secrets</param>
        public ResponseHandler(IHttpClientFactory httpClientFactory, IConfiguration configuration) 
        { 
            _movieGlueRequestHandler = new MovieGluRequestHandler(httpClientFactory, configuration);            
        }

        /// <summary>
        /// Main driver method for getting the response for MovieListAPI end-user
        /// </summary>
        /// <returns>List of movies with their attached theaters and showings. Ready to be serialized into JSON.</returns>
        public async Task<IEnumerable<Movie>> GetResponse()
        {
            List<Movie> movies = new List<Movie>();
            JsonElement nowPlayingContent = await _movieGlueRequestHandler.GetNowPlaying();

            foreach (var requestMovie in nowPlayingContent.GetProperty("films").EnumerateArray())
            {
                movies.Add(await BuildMovie(requestMovie));
            }
            return movies;
        }

        /// <summary>
        /// Helper method for sending extra request to MovieGlu for getting theater showings for specified movie
        /// </summary>
        /// <param name="jsonMovie">JsonElement of a movie returned from MovieGlu's filmsNowShowing call. Spec here: https://developer.movieglu.com/v2/api-index/filmsnowshowing/</param>
        /// <returns>Object representing a movie for my MovieList react application</returns>
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

            JsonElement showingsContent = await _movieGlueRequestHandler.GetShowings(movie.Id);

            return movie;
        }
    }
}
