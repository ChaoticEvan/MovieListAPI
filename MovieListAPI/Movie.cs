namespace MovieListAPI
{
    public class Movie
    {
        public int Id { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public string? Title { get; set; }
        public string? TrailerLink { get; set; }
        public string? PosterLink { get; set; }
        private Dictionary<int, Cinema> Cinemas { get; set; }

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
            if(Cinemas.ContainsKey(cinema.Id))
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