﻿namespace MySportsClubManager.Services.Data
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

        public async Task<Image> Add(IFormFile imageFile, string name)
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
            return await this.imagesRepository.All().Where(i => i.URL == imageUrl).FirstOrDefaultAsync();
        }
    }
}
