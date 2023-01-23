namespace MovieListAPI
{
    public class Cinema
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public float Distance { get; set; }
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
            // If the movie is contained in our showings & the time is in our list then return false
            bool ifContainsMovie = Times.ContainsKey(filmId);            
            if (ifContainsMovie)
            {
                bool ifContainsShowingTime = Times[filmId] != null && Times[filmId].Contains(startTime);
                return !ifContainsShowingTime;
            }

            if (ifContainsMovie)
            {
                Times[filmId].Add(startTime);
            }
            else
            {
                Times[filmId] = new HashSet<string> { startTime };
            }
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
