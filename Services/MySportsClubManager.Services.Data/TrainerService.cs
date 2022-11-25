namespace MySportsClubManager.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualBasic;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.Trainer;

    public class TrainerService : ITrainerService
    {
        private readonly IDeletableEntityRepository<Trainer> trainerRepository;
        private readonly IRepository<Club> clubRepository;

        public TrainerService(IDeletableEntityRepository<Trainer> trainerRepository, IRepository<Club> clubRepository)
        {
            this.trainerRepository = trainerRepository;
            this.clubRepository = clubRepository;
        }

        public async Task CreateAsync(string userId)
        {
            var trainer = new Trainer()
            {
                ApplicationUserId = userId,
            };

            await this.trainerRepository.AddAsync(trainer);
            await this.trainerRepository.SaveChangesAsync();
        }

        public async Task<int> GetTrainerIdAsync(string userId)
        {
            var trainerId = await this.trainerRepository.All()
                .Where(x => x.ApplicationUserId == userId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            return trainerId;
        }

        public async Task<ContactTrainerInputModel> GetTrainerInformationAsync(int trainerId)
        {
            var trainerInfo = await this.trainerRepository.All()
                .Where(x => x.Id == trainerId)
                .Include(x => x.ApplicationUser)
                .To<ContactTrainerInputModel>()
                .FirstOrDefaultAsync();

            return trainerInfo;
        }

        public async Task<bool> OwnsClub(string userId, int clubId)
        {
            var club = await this.clubRepository.All()
                .Where(x => x.Id == clubId)
                .FirstOrDefaultAsync();

            return club.TrainerId == await this.GetTrainerIdAsync(userId);
        }
    }
}
