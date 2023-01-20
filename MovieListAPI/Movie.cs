namespace MovieListAPI
{
    public class Movie
    {
        public DateOnly ReleaseDate { get; set; }
        public string? Title { get; set; }
        public string? Director { get; set; }
    }
}