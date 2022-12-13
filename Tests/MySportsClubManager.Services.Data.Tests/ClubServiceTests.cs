namespace MySportsClubManager.Services.Data.Tests
{
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
    using MySportsClubManager.Web.ViewModels.Club;
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Xunit;

    public class ClubServiceTests
    {
        private IDeletableEntityRepository<Club> clubsRepository;
        private IImageService imageService;
        private IAthleteService athleteService;
        private ClubService clubService;
        private IFormFile formFile;
        private ApplicationDbContext applicationDbContext;

        public ClubServiceTests()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MySportsClubManagerDbClub")
            .Options;

            this.applicationDbContext = new ApplicationDbContext(contextOptions);

            this.applicationDbContext.Database.EnsureDeleted();
            this.applicationDbContext.Database.EnsureCreated();

            this.clubsRepository = new EfDeletableEntityRepository<Club>(this.applicationDbContext);

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

            var mockAthleteService = new Mock<IAthleteService>();
            mockAthleteService.Setup(x => x.RegisterSportClubAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(async () =>
                {
                    await Task.Delay(1);
                    var athlete = await this.applicationDbContext.Athletes.Where(x => x.Id == 1).FirstOrDefaultAsync();

                    athlete.EnrolledClubId = 1;
                    this.applicationDbContext.Update(athlete);
                    await this.applicationDbContext.SaveChangesAsync();

                    return;
                });

            mockAthleteService.Setup(x => x.UnregisterSportClubAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(async () =>
                {
                    await Task.Delay(1);
                    var athlete = await this.applicationDbContext.Athletes.Where(x => x.Id == 1).FirstOrDefaultAsync();

                    athlete.EnrolledClubId = null;
                    this.applicationDbContext.Update(athlete);
                    await this.applicationDbContext.SaveChangesAsync();

                    return;
                });
            this.athleteService = mockAthleteService.Object;

            this.clubService = new ClubService(this.clubsRepository, this.imageService, this.athleteService);
        }

        [Fact]
        public void GetCountShouldWorkCorrectly()
        {
            this.SeedData();

            var count = this.clubService.GetCount();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task GetOneAsyncShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ClubDetailsViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            var club = await this.clubService.GetOneAsync<ClubDetailsViewModel>(1);

            Assert.Equal("Test", club.Name);
            Assert.Equal(1, club.Id);
            Assert.Equal("Test", club.Address);
        }

        [Fact]
        public async Task GetOneAsyncShouldThrowExWhenNotFound()
        {
            AutoMapperConfig.RegisterMappings(typeof(ClubDetailsViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            await Assert.ThrowsAsync<ArgumentException>(async () => { await this.clubService.GetOneAsync<ClubDetailsViewModel>(3); });
        }

        [Fact]
        public async Task EnrollShouldThrowExWhenClubNotFound()
        {
            this.SeedData();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => { await this.clubService.Enroll(3, "test"); });
        }

        [Fact]
        public async Task EnrollShouldWorkFine()
        {
            this.SeedData();

            await this.clubService.Enroll(1, "TestId");
            var athlete = await this.applicationDbContext.Athletes.Where(x => x.Id == 1).FirstOrDefaultAsync();
            Assert.Equal(1, athlete.EnrolledClubId);
        }

        [Fact]
        public async Task DisenrollShouldThrowExWhenClubNotFound()
        {
            this.SeedData();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => { await this.clubService.Disenroll(3, "test"); });
        }

        [Fact]
        public async Task DisenrollShouldWorkFine()
        {
            this.SeedData();

            await this.clubService.Enroll(1, "TestId");
            await this.clubService.Disenroll(1, "TestId");
            var athlete = await this.applicationDbContext.Athletes.Where(x => x.Id == 1).FirstOrDefaultAsync();

            Assert.Null(athlete.EnrolledClubId);
        }

        [Fact]
        public async Task GetClubsWithSameSportAsyncShouldWorkCorrectly()
        {
            this.SeedData();

            var clubs = await this.clubService.GetClubsWithSameSportAsync("Test");

            Assert.Single(clubs);
        }

        [Fact]
        public async Task GetAllForSearchAsyncShouldWorkCorrectly()
        {
            this.SeedData();

            var clubs = await this.clubService.GetAllForSearchAsync("Test");

            Assert.Single(clubs);
        }

        [Fact]
        public async Task GetMineAsyncShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ClubInListViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            var clubs = await this.clubService.GetMineAsync<ClubInListViewModel>(1);

            Assert.Single(clubs);
        }

        [Fact]
        public async Task AllCreatedAsyncShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ClubInListViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            var clubs = await this.clubService.AllCreatedAsync<ClubInListViewModel>(1, 1, 8);

            Assert.Single(clubs);
        }

        [Fact]
        public async Task AllAsyncShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ClubInListViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            var clubs = await this.clubService.AllAsync<ClubInListViewModel>(1, 8);

            Assert.Single(clubs);
        }

        [Fact]
        public async Task CreateShouldWorkCorrectly()
        {
            this.SeedData();

            var model = new CreateClubInputModel()
            {
                Name = "1",
                Description = "2",
                Fee = 2.31M,
                SportId = 1,
                Address = "aaa",
                ImageFile = this.formFile,
            };
            await this.clubService.CreateAsync(model, 1);
            var clubs = await this.clubService.AllAsync<ClubInListViewModel>(1, 8);
            Assert.Equal(2, clubs.Count);
        }

        [Fact]
        public async Task DeleteAsyncShouldWorkCorrectly()
        {
            this.SeedData();

            await this.clubService.DeleteAsync(1);
            var clubs = await this.clubService.AllAsync<ClubInListViewModel>(1, 8);
            Assert.Empty(clubs);
        }

        [Fact]
        public async Task DeleteAsyncShouldThrowExWhenNotFound()
        {
            this.SeedData();

            await Assert.ThrowsAsync<ArgumentException>(async () => { await this.clubService.DeleteAsync(3); });
        }

        [Fact]
        public async Task EditAsyncShouldWorkFineWhenFileIsUploaded()
        {
            this.SeedData();
            var model = new EditClubInputModel()
            {
                Id = 1,
                Name = "Test2",
                Description = "Test2",
                Address = "Address2",
                SportId = 1,
                Fee = 100.9M,
                ImageFile = this.formFile,
                ImageUrl = "TestUrl",
            };

            await this.clubService.EditAsync(model);
            var club = await this.applicationDbContext.Clubs.Where(x => x.Id == 1).FirstOrDefaultAsync();
            Assert.Equal(1, club.Id);
            Assert.Equal("Test2", club.Name);
            Assert.Equal("Address2", club.Address);
            Assert.Equal("testUrl", club.Image.URL);
        }

        [Fact]
        public async Task EditAsyncShouldThrowExWhenNotFound()
        {
            this.SeedData();
            var model = new EditClubInputModel()
            {
                Id = 3,
                Name = "Test2",
                Description = "Test2",
                Address = "Address2",
                SportId = 1,
                Fee = 100.9M,
                ImageFile = this.formFile,
                ImageUrl = "TestUrl",
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => { await this.clubService.EditAsync(model); });
        }

        [Fact]
        public async Task EditAsyncShouldWorkFineWhenUrlIsGiven()
        {
            this.SeedData();
            var model = new EditClubInputModel()
            {
                Id = 1,
                Name = "Test2",
                Description = "Test2",
                Address = "Address2",
                SportId = 1,
                Fee = 100.9M,
                ImageUrl = "TestUrl",
            };

            await this.clubService.EditAsync(model);
            var club = await this.applicationDbContext.Clubs.Where(x => x.Id == 1).FirstOrDefaultAsync();
            Assert.Equal(1, club.Id);
            Assert.Equal("Test2", club.Name);
            Assert.Equal("Address2", club.Address);
            Assert.Equal("testUrl", club.Image.URL);
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
                ApplicationUserId = user.Id,
            };
            var trainer = new Trainer()
            {
                Id = 1,
                ApplicationUser = user,
                ApplicationUserId = user.Id,
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
                Trainer = trainer,
                TrainerId = trainer.Id,
                Sport = sport,
                SportId = sport.Id,
                Image = image,
            };

            await this.applicationDbContext.Athletes.AddAsync(athlete);
            await this.applicationDbContext.Clubs.AddAsync(club);
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
