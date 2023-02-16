namespace MovieListAPI.CustomObjects
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
        /// Set of start times. Public for JSON Serialization
        /// </summary>
        public List<string> StartTimes { get; set; }

        public Cinema()
        {
            StartTimes = new List<string>();
        }
    }
}
