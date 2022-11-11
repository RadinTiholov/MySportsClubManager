namespace MySportsClubManager.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualBasic;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.ViewModels.Country;
    using MySportsClubManager.Web.ViewModels.Creator;
    using MySportsClubManager.Web.ViewModels.Sport;

    public class SportService : ISportService
    {
        private readonly IRepository<Sport> sportsRepository;
        private readonly IRepository<Creator> creatorsRepository;
        private readonly IRepository<Country> countryRepository;

        public SportService(IRepository<Sport> sportsRepository, IRepository<Creator> creatorsRepository, IRepository<Country> countryRepository)
        {
            this.sportsRepository = sportsRepository;
            this.creatorsRepository = creatorsRepository;
            this.countryRepository = countryRepository;
        }

        public async Task<List<SportViewModel>> AllAsync()
        {
            return await this.sportsRepository
                .All()
                .Select(s => new SportViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    CreationDate = s.CreationDate,
                    ImageUrl = s.ImageUrl,
                    Country = new CountryViewModel() { Id = s.CountryId, Name = s.Country.Name },
                    Creator = new CreatorViewModel() { Id = s.CreatorId, Name = s.Creator.Name },
                })
                .ToListAsync();
        }

        public async Task<List<SportListViewModel>> AllForInputAsync()
        {
            return await this.sportsRepository
                .All()
                .Select(x => new SportListViewModel
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

            var sport = new Sport()
            {
                Name = model.Name,
                Description = model.Description,
                CreationDate = model.CreationDate,
                Creator = creator,
                Country = country,
                ImageUrl = model.ImageUrl,
            };

            await this.sportsRepository.AddAsync(sport);
            await this.sportsRepository.SaveChangesAsync();
        }
    }
}
