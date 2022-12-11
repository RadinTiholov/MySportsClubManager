namespace MySportsClubManager.Services.Data.Tests
{
    using System;
    using System.IO;
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
            mockImageService.Setup(x => x.AddByFile(this.formFile, "test"))
                .Returns(async () =>
                {
                    await Task.Delay(1);
                    return new Image()
                    {
                        URL = "testUrl",
                    };
                });
            this.imageService = mockImageService.Object;

            this.sportService = new SportService(this.sportsRepository, this.creatorsRepository, this.countryRepository, this.imageService);
        }

        [Fact]
        public async Task AllAsyncShouldWorksCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SportInListViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            var sports = await this.sportService.AllAsync<SportInListViewModel>(1, 8);

            Assert.Single(sports);
        }

        [Fact]
        public async Task AllForInputShouldWorksCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SportInDropdownViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            var sports = await this.sportService.AllForInputAsync();

            Assert.Single(sports);
        }

        [Fact]
        public async Task GetRecentShouldWorksCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SportInDropdownViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            var sports = await this.sportService.GetRecentAsync();

            Assert.Single(sports);
        }

        [Fact]
        public async Task GetRecentFullShouldWorksCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SportInListViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            var sports = await this.sportService.GetRecentFullAsync();

            Assert.Single(sports);
        }

        [Fact]
        public void GetCountShouldWorksCorrectly()
        {
            this.SeedData();

            var count = this.sportService.GetCount();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task GetOneShouldWorksCorrectly()
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
            var fileName = "test.pdf";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            return new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
        }
    }
}
