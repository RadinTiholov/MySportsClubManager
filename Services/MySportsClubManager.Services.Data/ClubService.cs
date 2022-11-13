namespace MySportsClubManager.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.Club;

    using Club = MySportsClubManager.Data.Models.Club;

    public class ClubService : IClubService
    {
        private readonly IDeletableEntityRepository<Club> clubsRepository;

        public ClubService(IDeletableEntityRepository<Club> clubsRepository)
        {
            this.clubsRepository = clubsRepository;
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

        public async Task Create(CreateClubInputModel model, string ownerId)
        {
            var club = new Club()
            {
                Name = model.Name,
                Description = model.Description,
                SportId = model.SportId,
                OwnerId = ownerId,
                Address = model.Address,
                Fee = model.Fee,
                ImageUrl = model.ImageUrl,
            };

            await this.clubsRepository.AddAsync(club);
            await this.clubsRepository.SaveChangesAsync();
        }

        public int GetCount()
        {
            return this.clubsRepository.All().Count();
        }
    }
}
