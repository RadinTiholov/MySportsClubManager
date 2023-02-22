namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using MySportsClubManager.Web.ViewModels.Message;

    public interface IMessageService
    {
        Task CreateAsync(CreateMessageInputModel model);
    }
}
