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
    using MySportsClubManager.Web.ViewModels.Review;
    using MySportsClubManager.Web.ViewModels.Sport;
    using Xunit;

    public class SportServiceTests
    {
        private IDeletableEntityRepository<Sport> sportsRepository;
        private IRepository<Creator> creatorsRepository;
        private IRepository<Country> countryRepository;
        private ApplicationDbContext applicationDbContext;
        private IImageService imageService;
        private ISportService sportService;
        private IFormFile formFile;

        public SportServiceTests()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MySportsClubManagerDbSports")
            .Options;

            this.applicationDbContext = new ApplicationDbContext(contextOptions);

            this.applicationDbContext.Database.EnsureDeleted();
            this.applicationDbContext.Database.EnsureCreated();

            this.sportsRepository = new EfDeletableEntityRepository<Sport>(this.applicationDbContext);
            this.creatorsRepository = new EfRepository<Creator>(this.applicationDbContext);

            this.countryRepository = new EfRepository<Country>(this.applicationDbContext);

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

            this.sportService = new SportService(this.sportsRepository, this.creatorsRepository, this.countryRepository, this.imageService);
        }

        [Fact]
        public async Task AllAsyncShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SportInListViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            var sports = await this.sportService.AllAsync<SportInListViewModel>(1, 8);

            Assert.Single(sports);
        }

        [Fact]
        public async Task AllForInputShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SportInDropdownViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            var sports = await this.sportService.AllForInputAsync();

            Assert.Single(sports);
        }

        [Fact]
        public async Task GetRecentShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SportInDropdownViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            var sports = await this.sportService.GetRecentAsync();

            Assert.Single(sports);
        }

        [Fact]
        public async Task GetRecentFullShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SportInListViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            var sports = await this.sportService.GetRecentFullAsync();

            Assert.Single(sports);
        }

        [Fact]
        public void GetCountShouldWorkCorrectly()
        {
            this.SeedData();

            var count = this.sportService.GetCount();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task GetOneShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SportDetailsViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            var sport = await this.sportService.GetOneAsync<SportDetailsViewModel>(1);

            Assert.Equal("Test", sport.Name);
            Assert.Equal("Test", sport.Description);
        }

        [Fact]
        public async Task GetOneShouldThrowExWhenNotFound()
        {
            AutoMapperConfig.RegisterMappings(typeof(SportDetailsViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            await Assert.ThrowsAsync<ArgumentException>(async () => { await this.sportService.GetOneAsync<SportDetailsViewModel>(3); });
        }

        [Fact]
        public async Task DeletShouldThrowExWhenNotFound()
        {
            this.SeedData();

            await Assert.ThrowsAsync<ArgumentException>(async () => { await this.sportService.DeleteAsync(3); });
        }

        [Fact]
        public async Task CreateShouldWorkCorrectly()
        {
            this.SeedData();
            var model = new CreateSportInputModel()
            {
                Name = "Test2",
                Description = "Test2",
                CreationDate = DateTime.Now,
                ImageFile = this.CreateFakeFormFile(),
                Country = "Test",
                Creator = "Test",
            };
            await this.sportService.CreateAsync(model);
            var sports = await this.applicationDbContext.Sports.ToListAsync();
            Assert.Equal(2, sports.Count);
        }

        [Fact]
        public async Task CreateShouldWorkCorrectlyWhenCreator()
        {
            this.SeedData();
            var model = new CreateSportInputModel()
            {
                Name = "Test2",
                Description = "Test2",
                CreationDate = DateTime.Now,
                ImageFile = this.CreateFakeFormFile(),
                Country = "Test",
                Creator = "Test2",
            };
            await this.sportService.CreateAsync(model);
            var sports = await this.applicationDbContext.Sports.ToListAsync();
            var creators = await this.applicationDbContext.Creators.ToListAsync();
            Assert.Equal(2, creators.Count);
            Assert.Equal(2, sports.Count);
        }

        [Fact]
        public async Task CreateShouldWorkCorrectlyWhenCountry()
        {
            this.SeedData();
            var model = new CreateSportInputModel()
            {
                Name = "Test2",
                Description = "Test2",
                CreationDate = DateTime.Now,
                ImageFile = this.CreateFakeFormFile(),
                Country = "Test2",
                Creator = "Test",
            };
            await this.sportService.CreateAsync(model);
            var sports = await this.applicationDbContext.Sports.ToListAsync();
            var countries = await this.applicationDbContext.Countries.ToListAsync();
            Assert.Equal(2, countries.Count);
            Assert.Equal(2, sports.Count);
        }

        [Fact]
        public async Task EditShouldWorkCorrectlyWhenFileIsPassed()
        {
            this.SeedData();
            var model = new EditSportInputModel()
            {
                Id = 1,
                Name = "TestEdit",
                Description = "TestEdit",
                CreationDate = DateTime.Now,
                ImageFile = this.CreateFakeFormFile(),
                ImageUrl = "aaa.com",
                Country = "Test",
                Creator = "Test",
            };
            await this.sportService.EditAsync(model);
            var images = await this.applicationDbContext.Images.ToListAsync();
            var sport = await this.applicationDbContext.Sports.Where(x => x.Id == 1).FirstOrDefaultAsync();
            Assert.Equal("TestEdit", sport.Name);
            Assert.Equal("TestEdit", sport.Description);
            Assert.Equal(2, images.Count);
        }

        [Fact]
        public async Task EditShouldWorkCorrectlyWhenUrlIsPassed()
        {
            this.SeedData();
            var model = new EditSportInputModel()
            {
                Id = 1,
                Name = "TestEdit",
                Description = "TestEdit",
                CreationDate = DateTime.Now,
                ImageUrl = "aaa.com",
                Country = "Test",
                Creator = "Test",
            };
            await this.sportService.EditAsync(model);
            var images = await this.applicationDbContext.Images.ToListAsync();
            var sport = await this.applicationDbContext.Sports.Where(x => x.Id == 1).FirstOrDefaultAsync();
            Assert.Equal("TestEdit", sport.Name);
            Assert.Equal("TestEdit", sport.Description);
            Assert.Equal(2, images.Count);
        }

        [Fact]
        public async Task EditShouldWorkCorrectlyWhenCreatorDoesntExists()
        {
            this.SeedData();
            var model = new EditSportInputModel()
            {
                Id = 1,
                Name = "TestEdit",
                Description = "TestEdit",
                CreationDate = DateTime.Now,
                ImageUrl = "aaa.com",
                Country = "Test",
                Creator = "Test2",
            };
            await this.sportService.EditAsync(model);
            var images = await this.applicationDbContext.Images.ToListAsync();
            var sport = await this.applicationDbContext.Sports.Where(x => x.Id == 1).FirstOrDefaultAsync();
            var creators = await this.applicationDbContext.Creators.ToListAsync();
            Assert.Equal("TestEdit", sport.Name);
            Assert.Equal("TestEdit", sport.Description);
            Assert.Equal(2, images.Count);
            Assert.Equal(2, creators.Count);
        }

        [Fact]
        public async Task EditShouldWorkCorrectlyWhenCountryDoesntExists()
        {
            this.SeedData();
            var model = new EditSportInputModel()
            {
                Id = 1,
                Name = "TestEdit",
                Description = "TestEdit",
                CreationDate = DateTime.Now,
                ImageUrl = "aaa.com",
                Country = "Test2",
                Creator = "Test",
            };
            await this.sportService.EditAsync(model);
            var images = await this.applicationDbContext.Images.ToListAsync();
            var sport = await this.applicationDbContext.Sports.Where(x => x.Id == 1).FirstOrDefaultAsync();
            var country = await this.applicationDbContext.Countries.ToListAsync();
            Assert.Equal("TestEdit", sport.Name);
            Assert.Equal("TestEdit", sport.Description);
            Assert.Equal(2, images.Count);
            Assert.Equal(2, country.Count);
        }

        [Fact]
        public async Task EditShouldThrowExWhenDoesntExists()
        {
            this.SeedData();
            var model = new EditSportInputModel()
            {
                Id = 10,
                Name = "TestEdit",
                Description = "TestEdit",
                CreationDate = DateTime.Now,
                ImageUrl = "aaa.com",
                Country = "Test2",
                Creator = "Test",
            };
            await Assert.ThrowsAsync<ArgumentException>(async () => { await this.sportService.EditAsync(model); });
        }

        private async void SeedData()
        {
            var image = new Image()
            {
                Id = 1,
                URL = "aaa.com",
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

            await this.applicationDbContext.Sports.AddAsync(sport);
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
