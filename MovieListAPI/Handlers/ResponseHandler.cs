using MovieListAPI.CustomObjects;
using System.Text.Json;

namespace MovieListAPI.Handlers
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
        /// <param name="movie">JsonElement of a movie returned from MovieGlu's filmsNowShowing call. Spec here: https://developer.movieglu.com/v2/api-index/filmsnowshowing/</param>
        /// <returns>Object representing a movie for my MovieList react application</returns>
        private async Task<Movie> BuildMovie(JsonElement movie)
        {
            Movie movieObject = new Movie()
            {
                Id = movie.GetProperty("film_id").GetInt32(),
                Title = movie.GetProperty("film_name").ToString(),
                TrailerLink = movie.GetProperty("film_trailer").ToString(),
                ReleaseDate = DateOnly.Parse(movie.GetProperty("release_dates")[0].GetProperty("release_date").ToString()),
                PosterLink = movie.GetProperty("images").GetProperty("poster").GetProperty("1").GetProperty("medium").GetProperty("film_image").ToString()
            };

            JsonElement showingsContent = new JsonElement();
            try
            {
                showingsContent = await _movieGlueRequestHandler.GetShowings(movieObject.Id);
            }
            catch (NoContentException)
            {
                return movieObject;
            }

            foreach (var cinema in showingsContent.GetProperty("cinemas").EnumerateArray())
            {
                movieObject.AddCinema(BuildCinema(cinema));
            }

            return movieObject;
        }

        /// <summary>
        /// Helper method to build a Cinema C# object from the MovieGlu json response 
        /// </summary>
        /// <param name="movie">JsonElement of a cinema returned from MovieGlu's filmShowTimes call. Spec here: https://developer.movieglu.com/v2/api-index/filmShowTimes/</param>
        /// <returns>Object representing a cinema for my MovieList react application</returns>
        private Cinema BuildCinema(JsonElement cinema)
        {
            Cinema cinemaObject = new Cinema()
            {
                Id = cinema.GetProperty("cinema_id").GetInt32(),
                Name = cinema.GetProperty("cinema_name").ToString(),
                Distance = float.Parse(cinema.GetProperty("distance").ToString())
            };

            JsonElement showingsObject = cinema.GetProperty("showings").GetProperty("Standard");
            foreach (var showtime in showingsObject.GetProperty("times").EnumerateArray())
            {
                cinemaObject.Times.Add(showtime.GetProperty("start_time").ToString());
            }

            return cinemaObject;
        }
    }
}
