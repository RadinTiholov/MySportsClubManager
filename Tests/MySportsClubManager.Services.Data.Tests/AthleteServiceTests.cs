namespace MySportsClubManager.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MySportsClubManager.Data;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Data.Repositories;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.Athlete;
    using Xunit;

    public class AthleteServiceTests
    {
        private IDeletableEntityRepository<Athlete> athleteRepository;
        private IAthleteService athleteService;
        private ApplicationDbContext applicationDbContext;

        public AthleteServiceTests()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MySportsClubManagerDbAthletes")
            .Options;

            this.applicationDbContext = new ApplicationDbContext(contextOptions);

            this.applicationDbContext.Database.EnsureDeleted();
            this.applicationDbContext.Database.EnsureCreated();

            this.athleteRepository = new EfDeletableEntityRepository<Athlete>(this.applicationDbContext);
            this.athleteService = new AthleteService(this.athleteRepository);
        }

        [Fact]
        public async Task CreateAsyncShouldWorkCorrectly()
        {
            this.SeedData();

            await this.athleteService.CreateAsync("TestId2");

            var athletes = await this.applicationDbContext.Athletes.ToListAsync();

            Assert.Equal(3, athletes.Count);
        }

        [Fact]
        public async Task IsEnrolledInClubAsyncReturnsFalse()
        {
            this.SeedData();

            var result = await this.athleteService.IsEnrolledInClubAsync("TestId", 1);

            Assert.False(result);
        }

        [Fact]
        public async Task IsEnrolledInClubAsyncReturnsTrue()
        {
            this.SeedData();

            var result = await this.athleteService.IsEnrolledInClubAsync("TestId2", 1);

            Assert.True(result);
        }

        [Fact]
        public async Task GetAllForContestAsyncShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(AthleteInListViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            var athletes = await this.athleteService.GetAllForContestAsync(1);

            Assert.Empty(athletes);
        }

        [Fact]
        public async Task IsEnrolledInTrainingAsyncReturnsFalse()
        {
            this.SeedData();

            var result = await this.athleteService.IsEnrolledInTrainingAsync("TestId", 1);

            Assert.False(result);
        }

        [Fact]
        public async Task IsEnrolledInTrainingAsyncReturnsTrue()
        {
            this.SeedData();

            var result = await this.athleteService.IsEnrolledInTrainingAsync("TestId2", 1);

            Assert.True(result);
        }

        [Fact]
        public async Task IsEnrolledInAnyClubAsyncReturnsFalse()
        {
            this.SeedData();

            var result = await this.athleteService.IsEnrolledInAnyClubAsync("TestId");

            Assert.False(result);
        }

        [Fact]
        public async Task GetAthleteIdAsyncShouldWorkCorrectly()
        {
            this.SeedData();

            var result = await this.athleteService.GetAthleteIdAsync("TestId");

            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetMyClubShouldWorkCorrectly()
        {
            this.SeedData();

            var result = await this.athleteService.GetMyClub("TestId2");

            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetMyClubShouldThrowExWhenNotFound()
        {
            this.SeedData();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => { await this.athleteService.GetMyClub("TestId"); });
        }

        [Fact]
        public async Task RegisterSportClubAsyncShouldThrowExWhenNotFound()
        {
            this.SeedData();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => { await this.athleteService.RegisterSportClubAsync("TestaId", 1); });
        }

        [Fact]
        public async Task RegisterSportClubAsyncShouldWorkCorrectly()
        {
            this.SeedData();

            await this.athleteService.RegisterSportClubAsync("TestId", 1);
            var athlete = await this.applicationDbContext.Athletes.Where(x => x.Id == 1).FirstOrDefaultAsync();

            Assert.Equal(1, athlete.EnrolledClubId);
        }

        [Fact]
        public async Task RegisterSportClubAsyncShouldThrowExWhenAlreadyRegistered()
        {
            this.SeedData();

            await Assert.ThrowsAsync<ArgumentException>(async () => { await this.athleteService.RegisterSportClubAsync("TestId2", 1); });
        }

        [Fact]
        public async Task UnregisterSportClubAsyncShouldThrowExWhenNotFound()
        {
            this.SeedData();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => { await this.athleteService.UnregisterSportClubAsync("TestaId", 1); });
        }

        [Fact]
        public async Task UnregisterSportClubAsyncShouldWorkCorrectly()
        {
            this.SeedData();

            await this.athleteService.RegisterSportClubAsync("TestId", 1);
            await this.athleteService.UnregisterSportClubAsync("TestId", 1);
            var athlete = await this.applicationDbContext.Athletes.Where(x => x.Id == 1).FirstOrDefaultAsync();

            Assert.Null(athlete.EnrolledClubId);
        }

        [Fact]
        public async Task RegisterSportClubAsyncShouldThrowExWhenItIsntRegistered()
        {
            this.SeedData();

            await Assert.ThrowsAsync<ArgumentException>(async () => { await this.athleteService.UnregisterSportClubAsync("TestId", 1); });
        }

        private async void SeedData()
        {
            var image = new Image()
            {
                Id = 1,
                URL = "aaa.com",
            };
            var training = new Training()
            {
                Id = 1,
                Date = DateTime.Now,
                Duration = new TimeSpan(1000),
                Topic = "Test",
                ClubId = 1,
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
                LastName = "Testov",
                Email = "TestEmail",
                Image = image,
            };
            var athlete = new Athlete()
            {
                Id = 1,
                ApplicationUser = user,
            };
            var win = new Win()
            {
                Id = 1,
                ContestId = 1,
                AthleteId = 1,
                Place = 1,
            };
            athlete.Wins.Add(win);
            var athlete2 = new Athlete()
            {
                Id = 2,
                ApplicationUser = user2,
                EnrolledClubId = 1,
            };
            athlete2.Trainings.Add(training);

            await this.applicationDbContext.Wins.AddAsync(win);
            await this.applicationDbContext.Athletes.AddAsync(athlete);
            await this.applicationDbContext.Athletes.AddAsync(athlete2);
            await this.applicationDbContext.Users.AddAsync(user2);
            await this.athleteRepository.SaveChangesAsync();
        }
    }
}
