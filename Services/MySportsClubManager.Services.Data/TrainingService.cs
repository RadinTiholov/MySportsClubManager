namespace MySportsClubManager.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.Training;

    public class TrainingService : ITrainingService
    {
        private readonly IDeletableEntityRepository<Training> trainingRepository;
        private readonly IRepository<Athlete> athleteRepository;

        public TrainingService(IDeletableEntityRepository<Training> trainingRepository, IRepository<Athlete> athleteRepository)
        {
            this.trainingRepository = trainingRepository;
            this.athleteRepository = athleteRepository;
        }

        public async Task CreateAsync(CreateTrainingInputModel model)
        {
            var training = new Training()
            {
                Topic = model.Topic,
                Date = model.Date,
                Duration = model.Duration,
                ClubId = model.ClubId,
            };

            await this.trainingRepository.AddAsync(training);
            await this.trainingRepository.SaveChangesAsync();
        }

        public async Task EditAsync(EditTrainingInputModel model)
        {
            var training = await this.trainingRepository.All()
                .Where(x => x.Id == model.Id)
                .FirstOrDefaultAsync();

            if (training == null)
            {
                throw new ArgumentException();
            }

            training.Topic = model.Topic;
            training.Date = model.Date;
            training.Duration = model.Duration;
            training.ClubId = model.ClubId;

            this.trainingRepository.Update(training);
            await this.trainingRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int trainingId)
        {
            var training = await this.trainingRepository.All()
            .Where(c => c.Id == trainingId)
                .FirstOrDefaultAsync();

            if (training != null)
            {
                this.trainingRepository.Delete(training);
                await this.trainingRepository.SaveChangesAsync();
            }
        }

        public async Task<T> GetOneAsync<T>(int id)
        {
            var training = await this.trainingRepository.All()
                .Where(s => s.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            if (training == null)
            {
                throw new ArgumentException();
            }

            return training;
        }

        public async Task<List<TrainingInListViewModel>> GetAllForClubAsync(int clubId)
        {
            return await this.trainingRepository
                .All()
                .Where(x => x.ClubId == clubId)
                .OrderByDescending(x => x.Date)
                .To<TrainingInListViewModel>()
                .ToListAsync();
        }

        public async Task<List<TrainingInListViewModel>> GetAllClubsForUserAsync(string userId)
        {
            return await this.trainingRepository
                .All()
                .Where(x => x.EnrolledAthletes.Any(e => e.ApplicationUserId == userId))
                .OrderByDescending(x => x.Date)
                .To<TrainingInListViewModel>()
                .ToListAsync();
        }

        public async Task Enroll(int trainingId, string applicationUserId)
        {
            Training training = await this.trainingRepository.All()
            .Include(x => x.EnrolledAthletes)
            .Where(x => x.Id == trainingId)
            .FirstOrDefaultAsync();

            if (training != null)
            {
                if (training.EnrolledAthletes.Any(a => a.ApplicationUserId == applicationUserId))
                {
                    throw new ArgumentException();
                }

                var athlete = await this.athleteRepository.All()
                    .Where(a => a.ApplicationUserId == applicationUserId)
                    .FirstOrDefaultAsync();

                training.EnrolledAthletes.Add(athlete);
                this.trainingRepository.Update(training);
                await this.trainingRepository.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public async Task<int> GetClubId(int trainingId)
        {
            var trainig = await this.trainingRepository.All()
            .Where(x => x.Id == trainingId)
            .FirstOrDefaultAsync();

            if (trainig == null)
            {
                throw new ArgumentNullException();
            }

            return trainig.ClubId;
        }
    }
}
