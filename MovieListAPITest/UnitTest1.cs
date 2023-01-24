using MovieListAPI;

namespace MovieListAPITest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AddNewCinemaToMovieShouldReturnTrue()
        {
            Movie movie = CreateTestMovie(1, "Test Movie 1", new DateOnly(2000, 2, 15),
                        "https://trailerlink.com", "https://posterlink.com");
            Cinema cinema = CreateTestCinema(1, "Test Cinema 1", 10.05f);

            bool addResult = movie.AddCinema(cinema);

            Assert.That(addResult);

            Cinema returnedCinema = movie.GetCinema(cinema.Id);
            Assert.AreEqual(1, returnedCinema.Id);
            Assert.AreEqual("Test Cinema 1", returnedCinema.Name);
            Assert.AreEqual(10.05f, returnedCinema.Distance);
        }

        [Test]
        public void AddDuplicateCinemaToMovieShouldReturnFalse()
        {
            Movie movie = CreateTestMovie(1, "Test Movie 1", new DateOnly(2000, 2, 15),
                        "https://trailerlink.com", "https://posterlink.com");
            Cinema cinema = CreateTestCinema(1, "Test Cinema 1", 10.05f);

            movie.AddCinema(cinema);
            bool secondAddResult = movie.AddCinema(cinema);

            Assert.That(!secondAddResult);
        }

        [Test]
        public void AddNewShowingToCinemaShouldReturnTrue()
        {
            Movie movie = CreateTestMovie(1, "Test Movie 1", new DateOnly(2000, 2, 15),
                        "https://trailerlink.com", "https://posterlink.com");
            Cinema cinema = CreateTestCinema(1, "Test Cinema 1", 10.05f);

            bool firstAddShowingResult = cinema.AddShowing(movie.Id, "05:30");

            Assert.That(firstAddShowingResult);
            Assert.That(cinema.GetShowings(movie.Id).Contains("05:30"));
        }

        [Test]
        public void AddNumerousShowingsToCinemaShouldBehaveAsExpected()
        {
            Movie movie = CreateTestMovie(1, "Test Movie 1", new DateOnly(2000, 2, 15),
                        "https://trailerlink.com", "https://posterlink.com");
            Cinema cinema = CreateTestCinema(1, "Test Cinema 1", 10.05f);

            bool firstAddShowingResult = cinema.AddShowing(movie.Id, "05:30");
            bool secondAddShowingResult = cinema.AddShowing(movie.Id, "06:30");
            bool duplicateAddShowingResult = cinema.AddShowing(movie.Id, "05:30");

            Assert.That(firstAddShowingResult);
            Assert.That(secondAddShowingResult);
            Assert.That(!duplicateAddShowingResult);
            Assert.That(cinema.GetShowings(movie.Id).Contains("05:30"));
            Assert.That(cinema.GetShowings(movie.Id).Contains("06:30"));
        }

        [Test]
        public void AddDuplicateShowingShouldReturnFalse()
        {
            Movie movie = CreateTestMovie(1, "Test Movie 1", new DateOnly(2000, 2, 15), 
                                    "https://trailerlink.com", "https://posterlink.com");
            Cinema cinema = CreateTestCinema(1, "Test Cinema 1", 10.05f);

            cinema.AddShowing(movie.Id, "05:30");
            Assert.That(!cinema.AddShowing(movie.Id, "05:30"));
        }

        [Test]
        public void GetShowingWithIncorrectIdShouldThrowException()
        {
            Cinema cinema = new Cinema();
            Assert.Throws<KeyNotFoundException>(
                delegate { cinema.GetShowings(0); });
        }

        [Test]
        public void GetCinemaWithIncorrectIdShouldThrowException()
        {
            Movie movie = new Movie();
            Assert.Throws<KeyNotFoundException>(
                delegate { movie.GetCinema(0); });
        }

        private Cinema CreateTestCinema(int id, string name, float distance)
        {
            return new Cinema()
            {
                Id = id,
                Name = name,
                Distance = distance
            };
        }

        private Movie CreateTestMovie(int id, string title, DateOnly releaseDate, string trailerLink, string posterLink)
        {
            return new Movie()
            { 
                Id = id,
                Title = title,
                ReleaseDate = releaseDate,
                TrailerLink = trailerLink,
                PosterLink = posterLink
            };

        }
    }
}