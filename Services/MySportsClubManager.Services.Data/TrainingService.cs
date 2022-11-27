namespace MySportsClubManager.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;
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
    }
}
