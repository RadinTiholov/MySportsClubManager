namespace MySportsClubManager.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;

    public class ImageService : IImageService
    {
        private readonly IRepository<Image> imagesRepository;
        private readonly ICloudinaryService cloudinaryService;

        public ImageService(IRepository<Image> imagesRepository, ICloudinaryService cloudinaryService)
        {
            this.imagesRepository = imagesRepository;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<Image> AddByFile(IFormFile imageFile, string name)
        {
            // Save image to Cloudinary
            var imageUrl = await this.cloudinaryService
                .UploadAsync(imageFile, name);

            // Save image to database
            var existingImage = await this.imagesRepository.All().Where(i => i.URL == imageUrl).FirstOrDefaultAsync();
            if (existingImage != null)
            {
                return existingImage;
            }

            var image = new Image() { URL = imageUrl };
            await this.imagesRepository.AddAsync(image);
            await this.imagesRepository.SaveChangesAsync();
            return await this.GetByUrl(imageUrl);
        }

        public async Task<Image> AddByUrl(string url)
        {
            Image image = await this.GetByUrl(url);
            if (image != null)
            {
                return image;
            }

            image = new Image()
            {
                URL = url,
            };

            await this.imagesRepository.AddAsync(image);
            return await this.GetByUrl(image.URL);
        }

        private async Task<Image> GetByUrl(string url)
        {
            return await this.imagesRepository.All().Where(x => x.URL == url).FirstOrDefaultAsync();
        }
    }
}
