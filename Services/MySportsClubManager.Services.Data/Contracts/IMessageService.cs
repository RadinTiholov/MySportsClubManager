namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MySportsClubManager.Web.ViewModels.Message;

    public interface IMessageService
    {
        Task CreateAsync(CreateMessageInputModel model, string senderId);

        Task<List<MessageInListViewModel>> GetAllForUsersAsync(string senderUsername, string receiverUsername);
    }
}
