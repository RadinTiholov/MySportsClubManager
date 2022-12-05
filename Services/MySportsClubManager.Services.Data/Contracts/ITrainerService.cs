namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using MySportsClubManager.Web.ViewModels.Trainer;

    public interface ITrainerService
    {
        Task CreateAsync(string userId);

        Task<int> GetTrainerIdAsync(string userId);

        Task<bool> OwnsClubAsync(string userId, int clubId);

        Task<bool> OwnsTrainingAsync(string userId, int trainingId);

        Task<ContactTrainerInputModel> GetTrainerInformationAsync(int trainerId);

        Task ContactWithTrainerAsync(ContactTrainerInputModel model);
    }
}
