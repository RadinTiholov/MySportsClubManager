namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MySportsClubManager.Web.ViewModels.Training;

    public interface ITrainingService
    {
        Task CreateAsync(CreateTrainingInputModel model);

        Task<List<TrainingInListViewModel>> GetAllForClubAsync(int clubId);

        Task<List<TrainingInListViewModel>> GetAllClubsForUserAsync(string userId);

        Task DeleteAsync(int trainingId);

        Task<T> GetOneAsync<T>(int id);

        Task EditAsync(EditTrainingInputModel model);

        Task Enroll(int trainingId, string applicationUserId);

        Task<int> GetClubId(int trainingId);
    }
}
