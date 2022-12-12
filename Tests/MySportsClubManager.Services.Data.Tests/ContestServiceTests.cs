namespace MySportsClubManager.Services.Data.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using MySportsClubManager.Data;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Data.Repositories;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.Athlete;
    using MySportsClubManager.Web.ViewModels.Contest;
    using Xunit;

    public class ContestServiceTests
    {
        private IDeletableEntityRepository<Contest> contestRepository;
        private IRepository<Athlete> athleteRepository;
        private IRepository<Club> clubRepository;
        private IImageService imageService;
        private IContestService contestService;
        private IFormFile formFile;
        private ApplicationDbContext applicationDbContext;

        public ContestServiceTests()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MySportsClubManagerDbContest")
            .Options;

            this.applicationDbContext = new ApplicationDbContext(contextOptions);

            this.applicationDbContext.Database.EnsureDeleted();
            this.applicationDbContext.Database.EnsureCreated();

            this.contestRepository = new EfDeletableEntityRepository<Contest>(this.applicationDbContext);
            this.athleteRepository = new EfRepository<Athlete>(this.applicationDbContext);

            this.clubRepository = new EfRepository<Club>(this.applicationDbContext);
            this.clubRepository = new EfRepository<Club>(this.applicationDbContext);

            this.formFile = this.CreateFakeFormFile();
            var mockImageService = new Mock<IImageService>();
            mockImageService.Setup(x => x.AddByFile(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .Returns(async () =>
                {
                    await Task.Delay(1);
                    var image = new Image()
                    {
                        URL = "testUrl",
                    };
                    await this.applicationDbContext.Images.AddAsync(image);
                    await this.applicationDbContext.SaveChangesAsync();
                    return image;
                });
            mockImageService.Setup(x => x.AddByUrlAsync(It.IsAny<string>()))
                .Returns(async () =>
                {
                    await Task.Delay(1);
                    var image = new Image()
                    {
                        URL = "testUrl",
                    };
                    await this.applicationDbContext.Images.AddAsync(image);
                    await this.applicationDbContext.SaveChangesAsync();
                    return image;
                });
            this.imageService = mockImageService.Object;

            this.contestService = new ContestService(this.contestRepository, this.imageService, this.athleteRepository, this.clubRepository);
        }

        [Fact]
        public async Task AllAsyncShouldWorksCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ContestInListViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            var contests = await this.contestService.AllAsync<ContestInListViewModel>(1, 8);

            Assert.Equal(2, contests.Count);
        }

        [Fact]
        public async Task CreateContestShouldWorksCorrectly()
        {
            this.SeedData();
            var model = new CreateContestViewModel()
            {
                Name = "Test1",
                Description = "Test1",
                Address = "Test1",
                Date = DateTime.Now,
                SportId = 1,
                ImageFile = this.CreateFakeFormFile(),
            };

            await this.contestService.CreateAsync(model);

            var contests = await this.applicationDbContext.Contests.ToListAsync();
            Assert.Equal(3, contests.Count);
        }

        [Fact]
        public void GetCountShouldWorksCorrectly()
        {
            this.SeedData();

            Assert.Equal(2, this.contestService.GetCount());
        }

        [Fact]
        public async Task GetOneAsyncShouldWorksCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ContestDetailsViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            var contest = await this.contestService.GetOneAsync<ContestDetailsViewModel>(1);

            Assert.Equal(1, contest.Id);
            Assert.Equal("Contest", contest.Name);
        }

        [Fact]
        public async Task GetOneAsyncShouldThrowsExWhenWrongData()
        {
            AutoMapperConfig.RegisterMappings(typeof(ContestDetailsViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            await Assert.ThrowsAsync<ArgumentException>(async () => { await this.contestService.GetOneAsync<ContestDetailsViewModel>(3); });
        }

        [Fact]
        public async Task DeleteAsyncShouldWorksCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ContestDetailsViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            await this.contestService.DeleteAsync(1);

            var contests = await this.applicationDbContext.Contests.ToListAsync();

            Assert.Single(contests);
        }

        [Fact]
        public async Task DeleteShouldThrowsExWhenWrongData()
        {
            AutoMapperConfig.RegisterMappings(typeof(ContestDetailsViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            await Assert.ThrowsAsync<ArgumentException>(async () => { await this.contestService.DeleteAsync(3); });
        }

        [Fact]
        public async Task EditAsyncShouldWorksCorrectlyWhenFileIsUploaded()
        {
            this.SeedData();

            var model = new EditContestViewModel()
            {
                Id = 1,
                Name = "Contest1",
                Description = "Test1",
                Date = DateTime.Now,
                Address = "Test",
                SportId = 1,
                ImageFile = this.CreateFakeFormFile(),
                ImageUrl = "Aaaaa1",
            };

            await this.contestService.EditAsync(model);

            var contest = await this.applicationDbContext.Contests.Where(x => x.Id == 1).Include(x => x.Image).FirstOrDefaultAsync();
            Assert.Equal("Contest1", contest.Name);
            Assert.Equal("Test1", contest.Description);
            Assert.Equal("testUrl", contest.Image.URL);
        }

        [Fact]
        public async Task EditAsyncShouldWorksCorrectlyWhenFileIsntUploaded()
        {
            this.SeedData();

            var model = new EditContestViewModel()
            {
                Id = 1,
                Name = "Contest1",
                Description = "Test1",
                Date = DateTime.Now,
                Address = "Test",
                SportId = 1,
                ImageUrl = "testUrl",
            };

            await this.contestService.EditAsync(model);

            var contest = await this.applicationDbContext.Contests.Where(x => x.Id == 1).Include(x => x.Image).FirstOrDefaultAsync();
            Assert.Equal("Contest1", contest.Name);
            Assert.Equal("Test1", contest.Description);
            Assert.Equal("testUrl", contest.Image.URL);
        }

        [Fact]
        public async Task EditShouldThrowsExWhenWrongData()
        {
            AutoMapperConfig.RegisterMappings(typeof(ContestDetailsViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            var model = new EditContestViewModel()
            {
                Id = 4,
                Name = "Contest1",
                Description = "Test1",
                Date = DateTime.Now,
                Address = "Test",
                SportId = 1,
                ImageUrl = "Aaaaa1",
            };
            await Assert.ThrowsAsync<ArgumentException>(async () => { await this.contestService.EditAsync(model); });
        }

        [Fact]
        public async Task GetAllParticipantsAsyncShouldWorksCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(AthleteInDropdownViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            var participants = await this.contestService.GetAllParticipantsAsync(1);

            Assert.Empty(participants);
        }

        [Fact]
        public async Task RegisterAsyncShouldWorksCorrectly()
        {
            this.SeedData();

            await this.contestService.Register(1, "TestId");
            var participants = await this.contestService.GetAllParticipantsAsync(1);

            Assert.Single(participants);
        }

        [Fact]
        public async Task RegisterAsyncShouldThrowsExWhenAlreadyRegistered()
        {
            this.SeedData();

            await this.contestService.Register(1, "TestId");

            await Assert.ThrowsAsync<ArgumentException>(async () => { await this.contestService.Register(1, "TestId"); });
        }

        [Fact]
        public async Task RegisterAsyncShouldThrowsExWhenItIsOver()
        {
            this.SeedData();

            await this.contestService.Register(1, "TestId");

            await Assert.ThrowsAsync<ArgumentException>(async () => { await this.contestService.Register(2, "TestId"); });
        }

        [Fact]
        public async Task RegisterAsyncShouldThrowsExWhenTheContestIsNotFound()
        {
            this.SeedData();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => { await this.contestService.Register(10, "TestId"); });
        }

        [Fact]
        public async Task SetWinnersShouldWorksCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(AthleteInDropdownViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            var athlete1 = new Athlete()
            {
                Id = 10,
                ApplicationUserId = "10",
            };
            var athlete2 = new Athlete()
            {
                Id = 11,
                ApplicationUserId = "11",
            };
            var athlete3 = new Athlete()
            {
                Id = 12,
                ApplicationUserId = "12",
            };
            await this.applicationDbContext.Athletes.AddAsync(athlete1);
            await this.applicationDbContext.Athletes.AddAsync(athlete2);
            await this.applicationDbContext.Athletes.AddAsync(athlete3);
            await this.applicationDbContext.SaveChangesAsync();

            var contest = await this.applicationDbContext.Contests.Where(x => x.Id == 1).FirstOrDefaultAsync();
            contest.Participants.Add(athlete1);
            contest.Participants.Add(athlete2);
            contest.Participants.Add(athlete3);
            await this.applicationDbContext.SaveChangesAsync();
            await this.contestService.SetWinnersAsync(1, 10, 11, 12);
            var contestResult = await this.applicationDbContext.Contests.Where(x => x.Id == 1).Include(x => x.Participants).ThenInclude(x => x.Wins).FirstOrDefaultAsync();

            Assert.Equal(3, contestResult.Participants.Count);
            Assert.Equal(3, contestResult.Wins.Count);
        }

        [Fact]
        public async Task SetWinnersShouldWorksCorrectlyWhenThereAreAlreadyWinners()
        {
            AutoMapperConfig.RegisterMappings(typeof(AthleteInDropdownViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            var athlete1 = new Athlete()
            {
                Id = 10,
                ApplicationUserId = "10",
            };
            var athlete2 = new Athlete()
            {
                Id = 11,
                ApplicationUserId = "11",
            };
            var athlete3 = new Athlete()
            {
                Id = 12,
                ApplicationUserId = "12",
            };
            await this.applicationDbContext.Athletes.AddAsync(athlete1);
            await this.applicationDbContext.Athletes.AddAsync(athlete2);
            await this.applicationDbContext.Athletes.AddAsync(athlete3);
            await this.applicationDbContext.SaveChangesAsync();

            var contest = await this.applicationDbContext.Contests.Where(x => x.Id == 1).FirstOrDefaultAsync();
            contest.Participants.Add(athlete1);
            contest.Participants.Add(athlete2);
            contest.Participants.Add(athlete3);
            await this.applicationDbContext.SaveChangesAsync();
            await this.contestService.SetWinnersAsync(1, 11, 10, 12);

            await this.contestService.SetWinnersAsync(1, 10, 11, 12);
            var contestResult = await this.applicationDbContext.Contests.Where(x => x.Id == 1).Include(x => x.Participants).ThenInclude(x => x.Wins).FirstOrDefaultAsync();

            Assert.Equal(3, contestResult.Participants.Count);
            Assert.Equal(3, contestResult.Wins.Count);
        }

        [Fact]
        public async Task SetWinnersShouldThrowsExWhenTheContestIsNotFound()
        {
            this.SeedData();

            await Assert.ThrowsAsync<ArgumentException>(async () => { await this.contestService.SetWinnersAsync(9, 23, 34, 29); });
        }

        private async void SeedData()
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
                Email = "TestEmail",
                Image = image,
            };
            var athlete = new Athlete()
            {
                Id = 1,
                ApplicationUser = user,
            };
            var trainer = new Trainer()
            {
                Id = 1,
                ApplicationUser = user,
            };
            var creator = new Creator()
            {
                Id = 1,
                Name = "Test",
            };
            var country = new Country()
            {
                Id = 1,
                Name = "Test",
            };
            var sport = new Sport()
            {
                Id = 1,
                Name = "Test",
                Description = "Test",
                CreationDate = DateTime.Now,
                Image = image,
                ImageId = image.Id,
                Country = country,
                CountryId = country.Id,
                Creator = creator,
                CreatorId = creator.Id,
            };
            var club = new Club()
            {
                Id = 1,
                Name = "Test",
                Address = "Test",
                Description = "Test",
                Image = image,
                TrainerId = 1,
                Trainer = trainer,
            };
            var contest = new Contest()
            {
                Id = 1,
                Name = "Contest",
                Date = new DateTime(2032, 2, 2),
                Address = "Test",
                Sport = sport,
                SportId = sport.Id,
                Image = image,
                ImageId = image.Id,
                Description = "Test",
            };
            var contest2 = new Contest()
            {
                Id = 2,
                Name = "Contest2",
                Date = new DateTime(1900, 2, 2),
                Address = "Test",
                Sport = sport,
                SportId = sport.Id,
                Image = image,
                ImageId = image.Id,
                Description = "Test",
            };

            club.Athletes.Add(athlete);
            await this.applicationDbContext.Clubs.AddAsync(club);
            await this.applicationDbContext.Athletes.AddAsync(athlete);
            await this.applicationDbContext.AddAsync(contest);
            await this.applicationDbContext.AddAsync(contest2);
            await this.applicationDbContext.SaveChangesAsync();
        }

        private IFormFile CreateFakeFormFile()
        {
            var content = "Hello World from a Fake File";
            var fileName = "test";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            return new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
        }
    }
}
