namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Web.ViewModels.Review;

    public interface IReviewService
    {
        Task<ReviewViewModel> CreateAsync(string userId, int atheleteId, CreateReviewInputModel model);

        double GetAverageForClub(int clubId);
    }
}
