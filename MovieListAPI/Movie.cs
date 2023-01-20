namespace MovieListAPI
{
    public class Movie
    {
        public int Id { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public string? Title { get; set; }
        public string? TrailerLink { get; set; }
        public string? PosterLink { get; set; }
        public List<Cinema> Cinemas { get; set; }
    }

    public class Cinema
    {
        public List<string> Times { get; set; }
    }
}