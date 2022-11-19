﻿namespace MySportsClubManager.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;

    public class TrainerService : ITrainerService
    {
        private readonly IDeletableEntityRepository<Trainer> trainerRepository;
        private readonly IRepository<Club> clubRepository;

        public TrainerService(IDeletableEntityRepository<Trainer> trainerRepository, IRepository<Club> clubRepository)
        {
            this.trainerRepository = trainerRepository;
            this.clubRepository = clubRepository;
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

        public async Task<int> GetTrainerId(string userId)
        {
            var trainerId = await this.trainerRepository.All()
                .Where(x => x.ApplicationUserId == userId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            return trainerId;
        }

        public async Task<bool> OwnsClub(string userId, int clubId)
        {
            var club = await this.clubRepository.All()
                .Where(x => x.Id == clubId)
                .FirstOrDefaultAsync();

            return club.TrainerId == await this.GetTrainerId(userId);
        }
    }
}
