namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MySportsClubManager.Web.ViewModels.Training;

    public interface ITrainingService
    {
        Task CreateAsync(CreateTrainingInputModel model);

        Task<List<TrainingInListViewModel>> GetAllForClubAsync(int clubId);
    }
}
