namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using MySportsClubManager.Web.ViewModels.Trainer;

    public interface ITrainerService
    {
        Task CreateAsync(string userId);

        Task<int> GetTrainerIdAsync(string userId);

        Task<bool> OwnsClub(string userId, int clubId);

        Task<ContactTrainerInputModel> GetTrainerInformationAsync(int trainerId);

        Task ContactWithTrainerAsync(ContactTrainerInputModel model);
    }
}
