namespace MySportsClubManager.Services.Data.Tests
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MySportsClubManager.Data;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Data.Repositories;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.Review;
    using Xunit;

    public class ReviewServiceTests
    {
        private IDeletableEntityRepository<Review> reviewRepository;
        private IRepository<Club> clubRepository;
        private IRepository<ApplicationUser> applicationUserRepository;
        private IReviewService reviewService;
        private ApplicationDbContext applicationDbContext;

        public ReviewServiceTests()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MySportsClubManagerDb")
            .Options;

            this.applicationDbContext = new ApplicationDbContext(contextOptions);

            this.applicationDbContext.Database.EnsureDeleted();
            this.applicationDbContext.Database.EnsureCreated();

            this.reviewRepository = new EfDeletableEntityRepository<Review>(this.applicationDbContext);
            this.clubRepository = new EfRepository<Club>(this.applicationDbContext);
            this.applicationUserRepository = new EfRepository<ApplicationUser>(this.applicationDbContext);
            this.reviewService = new ReviewService(this.reviewRepository, this.clubRepository, this.applicationUserRepository);
        }

        [Fact]
        public async Task GetAllAsyncShouldWorkCorrectlyWithFirstPage()
        {
            AutoMapperConfig.RegisterMappings(typeof(ReviewInListViewModel).GetTypeInfo().Assembly);

            await this.SeedReview();

            var reviews = await this.reviewService.AllAsync(1, 8);

            Assert.Single(reviews);
        }

        [Fact]
        public async Task GetAllAsyncShouldWorkCorrectlyWithSecondPage()
        {
            AutoMapperConfig.RegisterMappings(typeof(ReviewInListViewModel).GetTypeInfo().Assembly);

            await this.SeedReview();

            var reviews = await this.reviewService.AllAsync(2, 8);

            Assert.Equal(0, reviews.Count);
        }

        [Fact]
        public async Task GetCountShouldWorkCorrectly()
        {
            await this.SeedReview();

            var count = this.reviewService.GetCount();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task GetAvarageForClubShouldWorkCorrectly()
        {
            await this.SeedReview();

            var rating = this.reviewService.GetAverageForClub(1);

            Assert.Equal(1, rating);
        }

        [Fact]
        public async Task GetAvarageForClubShouldReturnZeroWhenNotFound()
        {
            await this.SeedReview();

            var rating = this.reviewService.GetAverageForClub(5);

            Assert.Equal(0.0, rating);
        }

        [Fact]
        public async Task GetAllForAthleteShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ReviewInProfileViewModel).GetTypeInfo().Assembly);
            await this.SeedReview();

            var review = await this.reviewService.GetAllForAthleteAsync(1);

            Assert.Single(review);
        }

        [Fact]
        public async Task GetAllForClubAsyncShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ReviewViewModel).GetTypeInfo().Assembly);
            await this.SeedReview();

            var review = await this.reviewService.AllForClubAsync(1);

            Assert.Single(review);
        }

        [Fact]
        public async Task CreateReviewShouldWorkCorrectly()
        {
            await this.SeedReview();
            var model = new CreateReviewInputModel()
            {
                ClubId = 1,
                ReviewText = "a",
                Rating = 5,
            };
            ReviewViewModel result = await this.reviewService.CreateAsync("TestId2", 2, model);
            var count = this.reviewService.GetCount();

            Assert.Equal(2, count);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateReviewShouldThrowExeptionWhenUserAlreadyCreatedReview()
        {
            await this.SeedReview();
            var model = new CreateReviewInputModel()
            {
                ClubId = 1,
                ReviewText = "a",
                Rating = 5,
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await this.reviewService.CreateAsync("TestId1", 1, model));
        }

        [Fact]
        public async Task DeleteReviewShouldThrowExeptionWhenReviewIsNotFound()
        {
            await this.SeedReview();

            await Assert.ThrowsAsync<ArgumentException>(async () => await this.reviewService.DeleteAsync(3));
        }

        private async Task SeedReview()
        {
            var image = new Image()
            {
                Id = 1,
                URL = "aaa.com",
            };
            var user = new ApplicationUser()
            {
                Id = "TestId",
                UserName = "Test",
                FirstName = "Test",
                LastName = "Testov",
                Image = image,
            };
            var user2 = new ApplicationUser()
            {
                Id = "TestId2",
                UserName = "Test1",
                FirstName = "Test1",
                LastName = "Testov1",
                Image = image,
            };
            var athlete1 = new Athlete()
            {
                Id = 1,
                ApplicationUser = user,
            };
            var athlete2 = new Athlete()
            {
                Id = 2,
                ApplicationUser = user2,
            };
            var club = new Club()
            {
                Id = 1,
                Name = "Test",
                Address = "Test",
                Description = "Test",
                Image = image,
            };
            await this.applicationDbContext.Athletes.AddAsync(athlete2);
            await this.applicationDbContext.Reviews.AddAsync(new Review()
            {
                Id = 1,
                ReviewText = "a",
                Rating = 1,
                OwnerId = athlete1.Id,
                Owner = athlete1,
                ClubId = club.Id,
                Club = club,
            });
            await this.applicationDbContext.SaveChangesAsync();
        }
    }
}
