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

        public TrainingService(IDeletableEntityRepository<Training> trainingRepository)
        {
            this.trainingRepository = trainingRepository;
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

        public async Task<List<TrainingInListViewModel>> GetAllForClubAsync(int clubId)
        {
            return await this.trainingRepository
                .All()
                .Where(x => x.ClubId == clubId)
                .OrderByDescending(x => x.Date)
                .To<TrainingInListViewModel>()
                .ToListAsync();
        }
    }
}
