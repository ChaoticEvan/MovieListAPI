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
            Movie movie = new Movie()
            {
                Id = 1,
                Title = "Test Movie 1"
            };
            Cinema cinema = new Cinema()
            {
                Id = 1,
                Name = "Test Cinema 1"
            };

            bool addResult = movie.AddCinema(cinema);

            Assert.That(addResult);
            Assert.AreEqual(1, movie.GetCinema(cinema.Id).Id);
            Assert.AreEqual("Test Cinema 1", movie.GetCinema(cinema.Id).Name);
        }

        [Test]
        public void AddDuplicateCinemaToMovieShouldReturnFalse()
        {
            Movie movie = new Movie()
            {
                Id = 1,
                Title = "Test Movie 1"
            };
            Cinema cinema = new Cinema()
            {
                Id = 1,
                Name = "Test Cinema 1"
            };

            movie.AddCinema(cinema);
            bool secondAddResult = movie.AddCinema(cinema);

            Assert.That(!secondAddResult);
        }

        [Test]
        public void AddNewShowingToCinemaShouldReturnTrue()
        {
            Movie movie = new Movie()
            {
                Id = 1,
                Title = "Test Movie 1"
            };
            Cinema cinema = new Cinema()
            {
                Id = 1,
                Name = "Test Cinema 1"
            };

            bool firstAddShowingResult = cinema.AddShowing(movie.Id, "05:30");

            Assert.That(firstAddShowingResult);
            Assert.That(cinema.GetShowings(movie.Id).Contains("05:30"));
        }

        [Test]
        public void AddDuplicateShowingShouldReturnFalse()
        {
            Movie movie = new Movie()
            {
                Id = 1,
                Title = "Test Movie 1"
            };
            Cinema cinema = new Cinema()
            {
                Id = 1,
                Name = "Test Cinema 1"
            };

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
    }
}