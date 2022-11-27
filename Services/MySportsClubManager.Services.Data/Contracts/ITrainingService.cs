namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using MySportsClubManager.Web.ViewModels.Training;

    public interface ITrainingService
    {
        Task CreateAsync(CreateTrainingInputModel model);
    }
}
