namespace MySportsClubManager.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
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
        private readonly IAthleteService athleteService;

        public ClubService(IDeletableEntityRepository<Club> clubsRepository, IImageService imageService, IAthleteService athleteService)
        {
            this.clubsRepository = clubsRepository;
            this.imageService = imageService;
            this.athleteService = athleteService;
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
            var image = await this.imageService.AddByFile(model.ImageFile, model.ImageFile.FileName);

            var club = new Club()
            {
                Name = model.Name,
                Description = model.Description,
                SportId = model.SportId,
                TrainerId = trainerId,
                Address = model.Address,
                Fee = model.Fee,
                Image = image,
            };

            await this.clubsRepository.AddAsync(club);
            await this.clubsRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int clubId)
        {
            var club = await this.clubsRepository.All()
            .Where(c => c.Id == clubId)
                .FirstOrDefaultAsync();

            if (club == null)
            {
                throw new ArgumentException();
            }

            this.clubsRepository.Delete(club);
            await this.clubsRepository.SaveChangesAsync();
        }

        public async Task EditAsync(EditClubInputModel model)
        {
            var club = await this.clubsRepository.All()
                .Include(x => x.Image)
                .Where(x => x.Id == model.Id)
                .FirstOrDefaultAsync();

            if (club == null)
            {
                throw new ArgumentException();
            }

            Image image = null;
            if (model.ImageFile != null)
            {
                image = await this.imageService.AddByFile(model.ImageFile, model.ImageFile.FileName);
            }
            else
            {
                image = await this.imageService.AddByUrlAsync(model.ImageUrl);
            }

            club.Name = model.Name;
            club.Description = model.Description;
            club.SportId = model.SportId;
            club.Address = model.Address;
            club.Fee = model.Fee;
            club.Image = image;

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

        public async Task Enroll(int clubId, string userId)
        {
            var club = await this.clubsRepository.All()
            .Include(x => x.Image)
            .Where(x => x.Id == clubId)
            .FirstOrDefaultAsync();

            if (club != null)
            {
                if (club.Athletes.Any(a => a.ApplicationUserId == userId))
                {
                    throw new ArgumentException();
                }

                await this.athleteService.RegisterSportClubAsync(userId, clubId);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public async Task Disenroll(int clubId, string userId)
        {
            var club = await this.clubsRepository.All()
            .Include(x => x.Image)
            .Where(x => x.Id == clubId)
            .FirstOrDefaultAsync();

            if (club != null)
            {
                await this.athleteService.UnregisterSportClubAsync(userId, clubId);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public async Task<List<T>> AllCreatedAsync<T>(int page, int trainerId, int itemsPerPage = 8)
        {
            return await this.clubsRepository
                .AllAsNoTracking()
                .Where(x => x.TrainerId == trainerId)
                .OrderByDescending(x => x.CreatedOn)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .To<T>()
                .ToListAsync();
        }

        public async Task<List<T>> GetMineAsync<T>(int trainerId)
        {
            return await this.clubsRepository
               .AllAsNoTracking()
               .Where(x => x.TrainerId == trainerId)
               .OrderByDescending(x => x.CreatedOn)
               .To<T>()
               .ToListAsync();
        }

        public async Task<List<ClubInListViewModel>> GetAllForSearchAsync(string searchQuery)
        {
            return await this.clubsRepository
               .AllAsNoTracking()
               .Where(x => x.Name.Contains(searchQuery))
               .OrderByDescending(x => x.CreatedOn)
               .To<ClubInListViewModel>()
               .ToListAsync();
        }

        public async Task<List<ClubInListViewModel>> GetClubsWithSameSportAsync(string sportName)
        {
            return await this.clubsRepository
               .AllAsNoTracking()
               .Include(x => x.Sport)
               .Where(x => x.Sport.Name == sportName)
               .Take(4)
               .To<ClubInListViewModel>()
               .ToListAsync();
        }

        public async Task<List<ClubInListViewModel>> GetClubsWithHightestRating()
        {
            return await this.clubsRepository
               .All()
               .Include(x => x.Reviews)
               .OrderByDescending(x => x.Reviews.Average(x => x.Rating))
               .Take(4)
               .To<ClubInListViewModel>()
               .ToListAsync();
        }
    }
}
