namespace MySportsClubManager.Services.Data
{
    using System;
    using System.Collections.Generic;
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

        public async Task CreateAsync(CreateClubInputModel model, int trainerId)
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

                var imageUrl = await this.imageService.AddByFile(image, image.FileName);
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

        public async Task DeleteAsync(int clubId)
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

        public async Task EditAsync(EditClubInputModel model)
        {
            var club = await this.clubsRepository.All()
                .Include(x => x.Images)
                .Where(x => x.Id == model.Id)
                .FirstOrDefaultAsync();

            if (club == null)
            {
                throw new ArgumentException();
            }

            List<Image> images = new List<Image>();
            if (model.ImageFiles != null)
            {
                foreach (var image in model.ImageFiles)
                {
                    images.Add(await this.imageService.AddByFile(image, image.FileName));
                }
            }
            else
            {
                foreach (var image in club.Images)
                {
                    images.Add(await this.imageService.AddByUrlAsync(image.URL));
                }
            }

            club.Name = model.Name;
            club.Description = model.Description;
            club.SportId = model.SportId;
            club.Address = model.Address;
            club.Fee = model.Fee;
            club.Images = images;

            this.clubsRepository.Update(club);
            await this.clubsRepository.SaveChangesAsync();
        }

        public int GetCount()
        {
            return this.clubsRepository.All().Count();
        }

        public async Task<T> GetOneAsync<T>(int id)
        {
            var club = await this.clubsRepository.All()
                .Where(s => s.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            if (club == null)
            {
                throw new ArgumentException();
            }

            return club;
        }
    }
}
