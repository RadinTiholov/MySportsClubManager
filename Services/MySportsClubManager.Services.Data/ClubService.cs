namespace MySportsClubManager.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MySportsClubManager.Common;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.Club;

    using Club = MySportsClubManager.Data.Models.Club;

    public class ClubService : IClubService
    {
        private readonly IDeletableEntityRepository<Club> clubsRepository;
        private readonly IImageService imageService;

        public ClubService(IDeletableEntityRepository<Club> clubsRepository, IImageService imageService)
        {
            this.clubsRepository = clubsRepository;
            this.imageService = imageService;
        }

        public async Task<List<T>> AllAsync<T>(int page, int itemsPerPage = 8)
        {
            return await this.clubsRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .To<T>()
                .ToListAsync();
        }

        public async Task Create(CreateClubInputModel model, int trainerId)
        {
            var images = new List<Image>();
            foreach (var image in model.ImageFiles)
            {
                var extension = Path.GetExtension(image.FileName);
                var extensions = GlobalConstants.AllowedImageExtensions;

                if (!extensions.Contains(extension.ToLower()))
                {
                    throw new InvalidOperationException(GlobalConstants.AllowedExtensionsErrorMessage);
                }

                var imageUrl = await this.imageService.Add(image, image.FileName);
                images.Add(imageUrl);
            }

            var club = new Club()
            {
                Name = model.Name,
                Description = model.Description,
                SportId = model.SportId,
                TrainerId = trainerId,
                Address = model.Address,
                Fee = model.Fee,
                Images = images.ToArray(),
            };

            await this.clubsRepository.AddAsync(club);
            await this.clubsRepository.SaveChangesAsync();
        }

        public async Task Delete(int clubId)
        {
            var club = await this.clubsRepository.All()
            .Where(c => c.Id == clubId)
                .FirstOrDefaultAsync();
            if (club != null)
            {
                this.clubsRepository.Delete(club);
                await this.clubsRepository.SaveChangesAsync();
            }
        }

        public int GetCount()
        {
            return this.clubsRepository.All().Count();
        }

        public async Task<ClubDetailsViewModel> GetOne(int id)
        {
            var club = await this.clubsRepository.All()
                .To<ClubDetailsViewModel>()
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (club == null)
            {
                throw new ArgumentException();
            }

            return club;
        }
    }
}
