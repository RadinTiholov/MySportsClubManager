namespace MySportsClubManager.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.ViewModels.Club;

    using Club = MySportsClubManager.Data.Models.Club;

    public class ClubService : IClubService
    {
        private readonly IDeletableEntityRepository<Club> clubsRepository;

        public ClubService(IDeletableEntityRepository<Club> clubsRepository)
        {
            this.clubsRepository = clubsRepository;
        }

        public async Task<List<ClubViewModel>> AllAsync()
        {
            return await this.clubsRepository
                .All()
                .Select(c => new ClubViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Sport = c.Sport.Name,
                    Address = c.Address,
                    Fee = c.Fee,
                    ImageUrl = c.ImageUrl,
                })
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
    }
}
