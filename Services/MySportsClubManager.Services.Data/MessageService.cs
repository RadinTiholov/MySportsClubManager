namespace MySportsClubManager.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.Message;

    public class MessageService : IMessageService
    {
        private readonly IRepository<Message> messageRepository;
        private readonly IAthleteService athleteService;

        public MessageService(
            IRepository<Message> messageRepository,
            IAthleteService athleteService)
        {
            this.messageRepository = messageRepository;
            this.athleteService = athleteService;
        }

        public async Task CreateAsync(CreateMessageInputModel model, string senderId)
        {
            var receiverAthleteId = await this.athleteService.GetAthleteIdByUsernameAsync(model.ReceiverUsername);

            var senderAthleteId = await this.athleteService.GetAthleteIdAsync(senderId);

            var message = new Message()
            {
                Text = model.Text,
                ReceiverId = receiverAthleteId,
                SenderId = senderAthleteId,
            };

            await this.messageRepository.AddAsync(message);
            await this.messageRepository.SaveChangesAsync();
        }

        public async Task<List<MessageInListViewModel>> GetAllForUsersAsync(string senderUsername, string receiverUsername)
        {
            return await this.messageRepository
                .AllAsNoTracking()
                .Include(x => x.Sender)
                    .ThenInclude(s => s.ApplicationUser)
                .Include(x => x.Receiver)
                    .ThenInclude(r => r.ApplicationUser)
                .Where(x => (x.Sender.ApplicationUser.UserName == senderUsername
                    && x.Receiver.ApplicationUser.UserName == receiverUsername)
                    || (x.Sender.ApplicationUser.UserName == receiverUsername
                    && x.Receiver.ApplicationUser.UserName == senderUsername))
                .OrderBy(x => x.CreatedOn)
                .To<MessageInListViewModel>()
                .ToListAsync();
        }
    }
}
