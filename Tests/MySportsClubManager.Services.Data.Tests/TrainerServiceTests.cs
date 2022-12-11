namespace MySportsClubManager.Services.Data.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using Moq;
    using MySportsClubManager.Data;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Data.Repositories;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Services.Messaging;
    using MySportsClubManager.Web.ViewModels.Trainer;
    using MySportsClubManager.Web.ViewModels.Training;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Xunit;

    public class TrainerServiceTests
    {
        private IDeletableEntityRepository<Trainer> trainerRepository;
        private IRepository<Club> clubRepository;
        private IRepository<Training> trainingRepository;
        private IEmailSender emailSender;
        private IConfiguration configuration;
        private ITrainerService trainerService;
        private ApplicationDbContext applicationDbContext;

        public TrainerServiceTests()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MySportsClubManagerDbTrainers")
            .Options;

            this.applicationDbContext = new ApplicationDbContext(contextOptions);

            this.applicationDbContext.Database.EnsureDeleted();
            this.applicationDbContext.Database.EnsureCreated();

            this.trainingRepository = new EfRepository<Training>(this.applicationDbContext);
            this.clubRepository = new EfRepository<Club>(this.applicationDbContext);
            this.clubRepository = new EfRepository<Club>(this.applicationDbContext);
            this.trainerRepository = new EfDeletableEntityRepository<Trainer>(this.applicationDbContext);
            this.configuration = new Mock<IConfiguration>().Object;
            this.emailSender = new Mock<IEmailSender>().Object;
            this.trainerService = new TrainerService(this.trainerRepository, this.clubRepository, this.trainingRepository, this.emailSender, this.configuration);
        }

        [Fact]
        public async Task GetTrainerIdShouldWorksCorrectly()
        {
            await this.SeedData();

            var id = await this.trainerService.GetTrainerIdAsync("TestId");

            Assert.Equal(1, id);
        }

        [Fact]
        public async Task GetTrainerInformationAsyncShouldWorksCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ContactTrainerInputModel).GetTypeInfo().Assembly);
            await this.SeedData();

            var trainerInfo = await this.trainerService.GetTrainerInformationAsync(1);

            Assert.Equal("TestEmail", trainerInfo.TrainerEmail);
            Assert.Equal("Test", trainerInfo.FirstName);
            Assert.Equal("Testov", trainerInfo.LastName);
        }

        [Fact]
        public async Task OwnsClubAsyncShouldWorksCorrectlyWhenTheUserOwns()
        {
            await this.SeedData();

            var result = await this.trainerService.OwnsClubAsync("TestId", 1);

            Assert.True(result);
        }

        [Fact]
        public async Task OwnsClubAsyncShouldThrowExWhenIdIsWrong()
        {
            await this.SeedData();

            await Assert.ThrowsAsync<NullReferenceException>(async () => { await this.trainerService.OwnsClubAsync("TestId", 2); });
        }

        [Fact]
        public async Task OwnsTrainingAsyncShouldWorkCorrectly()
        {
            await this.SeedData();

            var result = await this.trainerService.OwnsTrainingAsync("TestId", 1);

            Assert.True(result);
        }

        [Fact]
        public async Task CreateAsyncTrainerShouldWorkCorrectly()
        {
            await this.SeedData();

            await this.trainerService.CreateAsync("TestId2");

            var trainer = await this.applicationDbContext.Trainers.Where(x => x.ApplicationUserId == "TestId2").FirstOrDefaultAsync();

            Assert.NotNull(trainer);
        }

        [Fact]
        public async Task CreateAsyncTrainerShouldNotCreateATrainer()
        {
            await this.SeedData();

            await this.trainerService.CreateAsync("TestId");

            var trainers = await this.applicationDbContext.Trainings.ToListAsync();

            Assert.Single(trainers);
        }

        private async Task SeedData()
        {
            this.applicationDbContext.Database.EnsureDeleted();
            this.applicationDbContext.Database.EnsureCreated();
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
                Email = "TestEmail",
                Image = image,
            };
            var user2 = new ApplicationUser()
            {
                Id = "TestId2",
                UserName = "Test2",
                FirstName = "Test2",
                LastName = "Testov2",
                Email = "TestEmail2",
                Image = image,
            };
            var club = new Club()
            {
                Id = 1,
                Name = "Test",
                Address = "Test",
                Description = "Test",
                Image = image,
                TrainerId = 1,
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
            var trainer = new Trainer()
            {
                Id = 1,
                ApplicationUser = user,
                ApplicationUserId = user.Id,
                OwnedClub = club,
                OwnedClubId = club.Id,
            };

            await this.applicationDbContext.Trainings.AddAsync(training);
            await this.applicationDbContext.Users.AddAsync(user2);
            await this.applicationDbContext.Trainers.AddAsync(trainer);
            await this.applicationDbContext.SaveChangesAsync();
        }
    }
}
