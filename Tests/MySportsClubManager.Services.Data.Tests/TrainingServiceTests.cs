namespace MySportsClubManager.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualBasic;
    using MySportsClubManager.Data;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Data.Repositories;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.Review;
    using MySportsClubManager.Web.ViewModels.Training;
    using Xunit;
    using static TorchSharp.torch.utils;

    public class TrainingServiceTests
    {
        private IDeletableEntityRepository<Training> trainingRepository;
        private IRepository<Athlete> athleteRepository;
        private TrainingService trainingService;
        private ApplicationDbContext applicationDbContext;

        public TrainingServiceTests()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MySportsClubManagerDbTrainings")
            .Options;

            this.applicationDbContext = new ApplicationDbContext(contextOptions);

            this.applicationDbContext.Database.EnsureDeleted();
            this.applicationDbContext.Database.EnsureCreated();

            this.trainingRepository = new EfDeletableEntityRepository<Training>(this.applicationDbContext);
            this.athleteRepository = new EfRepository<Athlete>(this.applicationDbContext);
            this.trainingService = new TrainingService(this.trainingRepository, this.athleteRepository);
        }

        [Fact]
        public async Task GetClubIdShouldWorkCorrectly()
        {
            await this.SeedTraining();

            var clubId = await this.trainingService.GetClubId(1);

            Assert.Equal(1, clubId);
        }

        [Fact]
        public async Task GetClubIdShouldReturnNullWhenIncorrectData()
        {
            await this.SeedTraining();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => { await this.trainingService.GetClubId(3); });
        }

        [Fact]
        public async Task EnrollShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(TrainingInListViewModel).GetTypeInfo().Assembly);
            await this.SeedTraining();

            await this.trainingService.Enroll(1, "TestId2");
            var trainigs = await this.trainingService.GetAllTrainingsForUserAsync("TestId2");

            Assert.Single(trainigs);
        }

        [Fact]
        public async Task EnrollShouldThrowExWhenIncorrectTrainingId()
        {
            await this.SeedTraining();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => { await this.trainingService.Enroll(5, "TestId2"); });
        }

        [Fact]
        public async Task EnrollShouldThrowExWhenUserIsEnrolled()
        {
            await this.SeedTraining();

            await Assert.ThrowsAsync<ArgumentException>(async () => { await this.trainingService.Enroll(1, "TestId"); });
        }

        [Fact]
        public async Task GetAllForClubAsyncShouldWorkCorrectly()
        {
            await this.SeedTraining();

            var trainings = await this.trainingService.GetAllForClubAsync(1);

            Assert.Single(trainings);
        }

        [Fact]
        public async Task GetOneShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(TrainingInListViewModel).GetTypeInfo().Assembly);
            await this.SeedTraining();

            var training = await this.trainingService.GetOneAsync<TrainingInListViewModel>(1);

            Assert.Equal(1, training.Id);
            Assert.Equal("Test", training.Topic);
        }

        [Fact]
        public async Task GetOneShouldThrowExWhenNotFound()
        {
            AutoMapperConfig.RegisterMappings(typeof(TrainingInListViewModel).GetTypeInfo().Assembly);
            await this.SeedTraining();

            await Assert.ThrowsAsync<ArgumentException>(async () => { await this.trainingService.GetOneAsync<TrainingInListViewModel>(5); });
        }

        [Fact]
        public async Task DeleteShouldWorkCorrectly()
        {
            await this.SeedTraining();

            await this.trainingService.DeleteAsync(1);
            var allTrainings = await this.applicationDbContext.Trainings.ToListAsync();

            Assert.Empty(allTrainings);
        }

        [Fact]
        public async Task CreateShouldWorkCorrectly()
        {
            await this.SeedTraining();

            var model = new CreateTrainingInputModel()
            {
                Date = DateTime.Now,
                Duration = new TimeSpan(1000),
                Topic = "Test",
                ClubId = 2,
            };
            await this.trainingService.CreateAsync(model);
            var allTrainings = await this.applicationDbContext.Trainings.ToListAsync();

            Assert.Equal(2, allTrainings.Count);
        }

        [Fact]
        public async Task EditShouldWorkCorrectly()
        {
            await this.SeedTraining();

            var model = new EditTrainingInputModel()
            {
                Id = 1,
                Date = new DateTime(2022, 2, 2),
                Duration = new TimeSpan(1000),
                Topic = "Test2",
                ClubId = 2,
            };
            await this.trainingService.EditAsync(model);
            var training = await this.applicationDbContext.Trainings.Where(x => x.Id == 1).FirstOrDefaultAsync();

            Assert.Equal("Test2", training.Topic);
            Assert.Equal(1, training.Id);
            Assert.Equal(2022, training.Date.Year);
            Assert.Equal(1000, training.Duration.Ticks);
        }

        [Fact]
        public async Task EditShouldThrowExWhenNotFound()
        {
            await this.SeedTraining();

            var model = new EditTrainingInputModel()
            {
                Id = 10,
                Date = new DateTime(2022, 2, 2),
                Duration = new TimeSpan(1000),
                Topic = "Test2",
                ClubId = 2,
            };
            await Assert.ThrowsAsync<ArgumentException>(async () => { await this.trainingService.EditAsync(model); });
        }

        private async Task SeedTraining()
        {
            this.applicationDbContext.Database.EnsureDeleted();
            this.applicationDbContext.Database.EnsureCreated();
            var image = new Image()
            {
                Id = 1,
                URL = "aaa.com",
            };
            var user2 = new ApplicationUser()
            {
                Id = "TestId2",
                UserName = "Test2",
                FirstName = "Test2",
                LastName = "Testov2",
                Image = image,
            };
            var athlete2 = new Athlete()
            {
                Id = 2,
                ApplicationUser = user2,
            };
            var user = new ApplicationUser()
            {
                Id = "TestId",
                UserName = "Test",
                FirstName = "Test",
                LastName = "Testov",
                Image = image,
            };
            var athlete = new Athlete()
            {
                Id = 1,
                ApplicationUser = user,
            };
            var club = new Club()
            {
                Id = 1,
                Name = "Test",
                Address = "Test",
                Description = "Test",
                Image = image,
            };
            var training = new Training()
            {
                Id = 1,
                Date = DateTime.Now,
                Duration = new TimeSpan(1000),
                Topic = "Test",
                Club = club,
                ClubId = club.Id,
            };
            training.EnrolledAthletes.Add(athlete);
            await this.applicationDbContext.Athletes.AddAsync(athlete2);
            await this.applicationDbContext.Trainings.AddAsync(training);
            await this.applicationDbContext.SaveChangesAsync();
        }
    }
}
