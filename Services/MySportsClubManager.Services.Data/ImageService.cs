namespace MySportsClubManager.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;

    public class ImageService : IImageService
    {
        private readonly IRepository<Image> imagesRepository;

        public ImageService(IRepository<Image> imagesRepository)
        {
            this.imagesRepository = imagesRepository;
        }

        public async Task<Image> Add(string imageUrl)
        {
            var existingImage = await this.imagesRepository.All().Where(i => i.URL == imageUrl).FirstOrDefaultAsync();
            if (existingImage != null)
            {
                return existingImage;
            }

            var image = new Image() { URL = imageUrl };
            await this.imagesRepository.AddAsync(image);
            await this.imagesRepository.SaveChangesAsync();
            return await this.imagesRepository.All().Where(i => i.URL == imageUrl).FirstOrDefaultAsync();
        }
    }
}
