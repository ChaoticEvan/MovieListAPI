using System.Runtime.Serialization;

namespace MovieListAPI.CustomObjects
{
    /// <summary>
    /// Custom object to represent movies in the Movie List API
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// MovieGlu ID of movie
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// First release date given for the movie
        /// </summary>
        public DateOnly ReleaseDate { get; set; }

        /// <summary>
        /// Movie title
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Link to a trailer
        /// </summary>
        public string? TrailerLink { get; set; }

        /// <summary>
        /// Link to a poster
        /// </summary>
        public string? PosterLink { get; set; }

        /// <summary>
        /// Map from Cinema ID -> Cinema object. Public for JSON Serialization
        /// </summary>
        public Dictionary<int, Cinema> Cinemas { get; set; }

        /// <summary>
        /// Constructs a movie object
        /// </summary>
        public Movie()
        {
            Cinemas = new Dictionary<int, Cinema>();
        }

        /// <summary>
        /// Adds cinema to this Movie's list of cinemas
        /// </summary>
        /// <param name="cinema">Cinema to add</param>
        /// <returns>True if cinema was added, false if cinema is contained in list</returns>
        public bool AddCinema(Cinema cinema)
        {
            if (Cinemas.ContainsKey(cinema.Id))
            {
                return false;
            }

            Cinemas.Add(cinema.Id, cinema);
            return true;
        }

        /// <summary>
        /// Gets cinema from this Movie's list of cinemas
        /// </summary>
        /// <param name="cinemaId">Id of cinema to get</param>
        /// <returns>Cinema specified in Id parameter. Null if cinema is not contained in list</returns>
        /// <exception cref="KeyNotFoundException">Throws when cinema ID is not contained in this movie's list.</exception>
        public Cinema GetCinema(int cinemaId)
        {
            if (!Cinemas.ContainsKey(cinemaId))
            {
                throw new KeyNotFoundException("That cinema is not showing this movie");
            }

            return Cinemas[cinemaId];
        }
    }
}