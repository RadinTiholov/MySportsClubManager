﻿namespace MySportsClubManager.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
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
                .Select(x => new SportInDropdownViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToListAsync();
        }

        public async Task Create(CreateSportInputModel model)
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

            var image = await this.imageService.Add(model.ImageFile, model.Name);

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

        public async Task Delete(int sportId)
        {
            var sport = await this.sportsRepository.AllAsNoTracking()
                .Where(s => s.Id == sportId)
                .FirstOrDefaultAsync();
            if (sport != null)
            {
                this.sportsRepository.Delete(sport);
                await this.sportsRepository.SaveChangesAsync();
            }
        }

        public int GetCount()
        {
            return this.sportsRepository.All().Count();
        }
    }
}
