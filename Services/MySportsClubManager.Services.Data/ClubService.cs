namespace MySportsClubManager.Services.Data
{
    using System.Threading.Tasks;

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
