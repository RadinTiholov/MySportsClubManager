namespace MySportsClubManager.Services.Data
{
    using System.Threading.Tasks;

    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;

    public class TrainerService : ITrainerService
    {
        private readonly IDeletableEntityRepository<Trainer> trainerRepository;

        public TrainerService(IDeletableEntityRepository<Trainer> trainerRepository)
        {
            this.trainerRepository = trainerRepository;
        }

        public async Task Create(string userId)
        {
            var trainer = new Trainer()
            {
                ApplicationUserId = userId,
            };

            await this.trainerRepository.AddAsync(trainer);
            await this.trainerRepository.SaveChangesAsync();
        }
    }
}
