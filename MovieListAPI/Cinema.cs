namespace MovieListAPI
{
    /// <summary>
    /// Custom object to represent cinemas in the Movie List API
    /// </summary>
    public class Cinema
    {
        /// <summary>
        /// MovieGlu ID of cinema
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Cinema name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Distance of Cinema to latitude and longitude coordinates
        /// </summary>
        public float Distance { get; set; }

        /// <summary>
        /// Private map of movie ID -> set of start times
        /// </summary>
        private Dictionary<int, HashSet<string>> Times;

        public Cinema()
        {
            Times = new Dictionary<int, HashSet<string>>();
        }

        /// <summary>
        /// Adds a movie showing time to this cinema's list of movie times
        /// </summary>
        /// <param name="filmId">MovieGlu ID of film to set time of</param>
        /// <param name="startTime">Starting time of movie</param>
        /// <returns>True if showing was added. False otherwise</returns>
        public bool AddShowing(int filmId, string startTime)
        {
            bool ifContainsMovie = Times.ContainsKey(filmId);            
            if (!ifContainsMovie)
            {
                Times.Add(filmId, new HashSet<string>() { startTime });
                return true;
            }

            bool ifContainsShowingTime = Times[filmId].Contains(startTime);
            if(ifContainsShowingTime)
            {
                return false;
            }

            Times[filmId].Add(startTime);
            return true;

        }

        /// <summary>
        /// Gets the set of showings for the movie specified
        /// </summary>
        /// <param name="filmId">MovieGlu Id of movie to get showings for</param>
        /// <returns>Set of strings that are start times for specified movie. Null otherwise</returns>
        /// <exception cref="KeyNotFoundException">Throws when film ID is not contained in this theater's showings list.</exception>
        public HashSet<string> GetShowings(int filmId)
        {
            if (!Times.ContainsKey(filmId))
            {
                throw new KeyNotFoundException("That film has no showings in this theater");
            }

            return Times[filmId];
        }
    }
}
