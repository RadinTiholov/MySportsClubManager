namespace MySportsClubManager.Services.Data
{
    using System.Threading.Tasks;

    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;

    public class AthleteService : IAthleteService
    {
        private readonly IDeletableEntityRepository<Athlete> athleteRepository;

        public AthleteService(IDeletableEntityRepository<Athlete> athleteRepository)
        {
            this.athleteRepository = athleteRepository;
        }

        public async Task Create(string userId)
        {
            var athlete = new Athlete()
            {
                ApplicationUserId = userId,
            };

            await this.athleteRepository.AddAsync(athlete);
            await this.athleteRepository.SaveChangesAsync();
        }
    }
}
