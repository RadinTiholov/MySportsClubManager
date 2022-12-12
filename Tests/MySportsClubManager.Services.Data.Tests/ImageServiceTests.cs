namespace MySportsClubManager.Services.Data.Tests
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using MySportsClubManager.Data;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Data.Repositories;
    using MySportsClubManager.Services.Data.Contracts;
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Tensorboard;
    using Xunit;

    public class ImageServiceTests
    {
        private IRepository<Image> imagesRepository;
        private ICloudinaryService cloudinaryService;
        private IImageService imageService;
        private ApplicationDbContext applicationDbContext;

        public ImageServiceTests()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MySportsClubManagerDbImages")
            .Options;

            this.applicationDbContext = new ApplicationDbContext(contextOptions);

            this.applicationDbContext.Database.EnsureDeleted();
            this.applicationDbContext.Database.EnsureCreated();

            this.imagesRepository = new EfRepository<Image>(this.applicationDbContext);
            var mockCloudinaryService = new Mock<ICloudinaryService>();
            mockCloudinaryService.Setup(x => x.UploadAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .Returns(async () =>
                {
                    await Task.Delay(1);
                    return "testUrl";
                });
            this.cloudinaryService = mockCloudinaryService.Object;

            this.imageService = new ImageService(this.imagesRepository, this.cloudinaryService);
        }

        [Fact]
        public async Task AddByUrlAsyncShouldWorksCorrectly()
        {
            this.SeedData();

            var image = await this.imageService.AddByUrlAsync("3333");
            var images = await this.applicationDbContext.Images.ToListAsync();
            Assert.Equal(3, images.Count);
        }

        [Fact]
        public async Task AddByUrlAsyncShouldWorksFine()
        {
            this.SeedData();

            var image = await this.imageService.AddByUrlAsync("1111");
            var images = await this.applicationDbContext.Images.ToListAsync();
            Assert.Equal(2, images.Count);
            Assert.Equal("1111", image.URL);
        }

        [Fact]
        public async Task AddByFileShouldReturnsImageWhenExists()
        {
            this.SeedData();

            var file = this.CreateFakeFormFile();
            var image = await this.imageService.AddByFile(file, "test2");

            var images = await this.applicationDbContext.Images.ToListAsync();
            Assert.Equal(3, images.Count);
        }

        private async void SeedData()
        {
            var firstImage = new Image()
            {
                Id = 1,
                URL = "1111",
            };
            var secondImage = new Image()
            {
                Id = 2,
                URL = "2222",
            };
            await this.applicationDbContext.Images.AddAsync(firstImage);
            await this.applicationDbContext.Images.AddAsync(secondImage);
            await this.applicationDbContext.SaveChangesAsync();
        }

        private IFormFile CreateFakeFormFile()
        {
            var content = "Fake";
            var fileName = "test1";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            return new FormFile(stream, 0, stream.Length, "id", fileName);
        }
    }
}
