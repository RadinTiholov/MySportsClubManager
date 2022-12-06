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
    using MySportsClubManager.Web.ViewModels.Sport;

    public class SportService : ISportService
    {
        private readonly IDeletableEntityRepository<Sport> sportsRepository;
        private readonly IRepository<Creator> creatorsRepository;
        private readonly IRepository<Country> countryRepository;
        private readonly IImageService imageService;

        public SportService(IDeletableEntityRepository<Sport> sportsRepository, IRepository<Creator> creatorsRepository, IRepository<Country> countryRepository, IImageService imageService)
        {
            this.sportsRepository = sportsRepository;
            this.creatorsRepository = creatorsRepository;
            this.countryRepository = countryRepository;
            this.imageService = imageService;
        }

        public async Task<List<T>> AllAsync<T>(int page, int itemsPerPage = 8)
        {
            return await this.sportsRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .To<T>()
                .ToListAsync();
        }

        public async Task<List<SportInDropdownViewModel>> AllForInputAsync()
        {
            return await this.sportsRepository
                .All()
                .To<SportInDropdownViewModel>()
                .ToListAsync();
        }

        public async Task CreateAsync(CreateSportInputModel model)
        {
            var creator = await this.creatorsRepository.All().FirstOrDefaultAsync(x => x.Name == model.Creator);
            var country = await this.countryRepository.All().FirstOrDefaultAsync(x => x.Name == model.Country);
            if (creator == null)
            {
                creator = new Creator()
                {
                    Name = model.Creator,
                };
                await this.creatorsRepository.AddAsync(creator);
                await this.creatorsRepository.SaveChangesAsync();
            }

            if (country == null)
            {
                country = new Country()
                {
                    Name = model.Country,
                };
                await this.countryRepository.AddAsync(country);
                await this.countryRepository.SaveChangesAsync();
            }

            var image = await this.imageService.AddByFile(model.ImageFile, model.ImageFile.FileName);

            var sport = new Sport()
            {
                Name = model.Name,
                Description = model.Description,
                CreationDate = model.CreationDate,
                Creator = creator,
                Country = country,
                Image = image,
            };

            await this.sportsRepository.AddAsync(sport);
            await this.sportsRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int sportId)
        {
            var sport = await this.sportsRepository.AllAsNoTracking()
                .Where(s => s.Id == sportId)
                .FirstOrDefaultAsync();

            if (sport == null)
            {
                throw new ArgumentException();
            }

            this.sportsRepository.Delete(sport);
            await this.sportsRepository.SaveChangesAsync();
        }

        public async Task EditAsync(EditSportInputModel model)
        {
            var sport = await this.sportsRepository.All()
                .Where(x => x.Id == model.Id)
                .FirstOrDefaultAsync();

            if (sport == null)
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

            var creator = await this.creatorsRepository.All().FirstOrDefaultAsync(x => x.Name == model.Creator);
            var country = await this.countryRepository.All().FirstOrDefaultAsync(x => x.Name == model.Country);
            if (creator == null)
            {
                creator = new Creator()
                {
                    Name = model.Creator,
                };
                await this.creatorsRepository.AddAsync(creator);
                await this.creatorsRepository.SaveChangesAsync();
            }

            if (country == null)
            {
                country = new Country()
                {
                    Name = model.Country,
                };
                await this.countryRepository.AddAsync(country);
                await this.countryRepository.SaveChangesAsync();
            }

            sport.Name = model.Name;
            sport.Description = model.Description;
            sport.CreationDate = model.CreationDate;
            sport.Image = image;
            sport.Creator = creator;
            sport.Country = country;

            this.sportsRepository.Update(sport);
            await this.sportsRepository.SaveChangesAsync();
        }

        public int GetCount()
        {
            return this.sportsRepository.AllAsNoTracking().Count();
        }

        public async Task<T> GetOneAsync<T>(int id)
        {
            var sport = await this.sportsRepository.All()
                .Where(s => s.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            if (sport == null)
            {
                throw new ArgumentException();
            }

            return sport;
        }

        public async Task<List<SportInDropdownViewModel>> GetRecent()
        {
            return await this.sportsRepository.AllAsNoTracking()
                .OrderBy(x => x.CreatedOn)
                .Take(10)
                .To<SportInDropdownViewModel>()
                .ToListAsync();
        }
    }
}
