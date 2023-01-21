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
    }
}
