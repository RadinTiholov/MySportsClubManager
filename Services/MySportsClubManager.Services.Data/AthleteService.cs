namespace MySportsClubManager.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.Athlete;
    using MySportsClubManager.Web.ViewModels.Win;

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

        public async Task<List<AthleteInListViewModel>> GetAllForContestAsync(int clubId)
        {
            return await this.athleteRepository.AllAsNoTracking()
                .Include(a => a.Contests)
                .Where(a => a.Contests.Any(x => x.Id == clubId))
                .To<AthleteInListViewModel>()
                .ToListAsync();
        }

        public async Task<bool> IsEnrolledInClubAsync(string userId, int clubId)
        {
            var athlete = await this.GetOneAsync(userId);

            return athlete.EnrolledClubId == clubId;
        }

        public async Task<bool> IsEnrolledInTrainingAsync(string userId, int trainingId)
        {
            var athlete = await this.GetOneAsync(userId);

            return athlete.Trainings.Any(x => x.Id == trainingId);
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

        public async Task<int> GetMyClub(string userId)
        {
            var athlete = await this.GetOneAsync(userId);
            if (athlete.EnrolledClubId == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                return (int)athlete.EnrolledClubId;
            }
        }

        public async Task<int> GetAthleteIdAsync(string userId)
        {
            var athlete = await this.GetOneAsync(userId);
            return athlete.Id;
        }

        public async Task<int> GetAthleteIdByUsernameAsync(string username)
        {
            var athlete = await this.athleteRepository
                .AllAsNoTracking()
                .Include(x => x.ApplicationUser)
                .Where(x => x.ApplicationUser.UserName == username)
                .FirstOrDefaultAsync();

            return athlete.Id;
        }

        public async Task<List<AchievementInListViewModel>> GetAllAchievementsForAthleteAsync(int athleteId)
        {
            var athlete = await this.athleteRepository.AllAsNoTracking()
                .Include(x => x.Wins)
                .ThenInclude(x => x.Contest)
                .Where(x => x.Id == athleteId)
                .FirstOrDefaultAsync();

            return athlete.Wins.Select(x => new AchievementInListViewModel()
            {
                ContestId = x.ContestId,
                Name = x.Contest.Name,
                Place = x.Place,
            }).ToList();
        }

        private async Task<Athlete> GetOneAsync(string userId)
        {
            return await this.athleteRepository.All()
                .Include(x => x.Trainings)
                .Where(x => x.ApplicationUserId == userId)
                .FirstOrDefaultAsync();
        }
    }
}
