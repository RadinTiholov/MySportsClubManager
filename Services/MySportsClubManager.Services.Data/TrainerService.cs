namespace MySportsClubManager.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Services.Messaging;
    using MySportsClubManager.Web.ViewModels.Trainer;

    public class TrainerService : ITrainerService
    {
        private readonly IDeletableEntityRepository<Trainer> trainerRepository;
        private readonly IRepository<Club> clubRepository;
        private readonly IEmailSender emailSender;
        private readonly IConfiguration configuration;

        public TrainerService(IDeletableEntityRepository<Trainer> trainerRepository, IRepository<Club> clubRepository, IEmailSender emailSender, IConfiguration configuration)
        {
            this.trainerRepository = trainerRepository;
            this.clubRepository = clubRepository;
            this.emailSender = emailSender;
            this.configuration = configuration;
        }

        public async Task ContactWithTrainerAsync(ContactTrainerInputModel model)
        {
            string email = model.EmailText + "\n" + "from: " + model.SenderEmail;
            await this.emailSender.SendEmailAsync(this.configuration["SendGrid:Email"], this.configuration["SendGrid:Sender"], model.TrainerEmail, model.Topic, email);
        }

        public async Task CreateAsync(string userId)
        {
            var trainer = await this.trainerRepository.All()
                .Where(x => x.ApplicationUserId == userId)
                .FirstOrDefaultAsync();

            if (trainer == null)
            {
                trainer = new Trainer()
                {
                    ApplicationUserId = userId,
                };

                await this.trainerRepository.AddAsync(trainer);
                await this.trainerRepository.SaveChangesAsync();
            }
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
