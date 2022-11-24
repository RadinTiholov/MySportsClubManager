namespace MySportsClubManager.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Schema;

    using Microsoft.EntityFrameworkCore;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Migrations;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;

    public class AthleteService : IAthleteService
    {
        private readonly IDeletableEntityRepository<Athlete> athleteRepository;

        public AthleteService(IDeletableEntityRepository<Athlete> athleteRepository)
        {
            this.athleteRepository = athleteRepository;
        }

        public async Task CreateAsync(string userId)
        {
            var athlete = new Athlete()
            {
                ApplicationUserId = userId,
            };

            await this.athleteRepository.AddAsync(athlete);
            await this.athleteRepository.SaveChangesAsync();
        }

        public async Task<bool> IsEnrolledInClubAsync(string userId, int clubId)
        {
            var athlete = await this.GetOneAsync(userId);

            return athlete.EnrolledClubId == clubId;
        }

        public async Task<bool> IsEnrolledInAnyClubAsync(string userId)
        {
            var athlete = await this.GetOneAsync(userId);

            return athlete.EnrolledClubId != null;
        }

        public async Task RegisterSportClubAsync(string userId, int clubId)
        {
            var athlete = await this.GetOneAsync(userId);

            if (athlete != null)
            {
                if (athlete.EnrolledClubId == null)
                {
                    athlete.EnrolledClubId = clubId;

                    this.athleteRepository.Update(athlete);
                    await this.athleteRepository.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public async Task UnregisterSportClubAsync(string userId, int clubId)
        {
            var athlete = await this.GetOneAsync(userId);

            if (athlete != null)
            {
                if (athlete.EnrolledClubId == null || athlete.EnrolledClubId != clubId)
                {
                    throw new ArgumentException();
                }
                else
                {
                    athlete.EnrolledClubId = null;

                    this.athleteRepository.Update(athlete);
                    await this.athleteRepository.SaveChangesAsync();
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        private async Task<Athlete> GetOneAsync(string userId)
        {
            return await this.athleteRepository.All()
                .Where(x => x.ApplicationUserId == userId)
                .FirstOrDefaultAsync();
        }
    }
}
