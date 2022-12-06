namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MySportsClubManager.Web.ViewModels.Review;

    public interface IReviewService
    {
        Task<ReviewViewModel> CreateAsync(string userId, int atheleteId, CreateReviewInputModel model);

        double GetAverageForClub(int clubId);

        Task<List<ReviewViewModel>> AllForClubAsync(int clubId);

        Task<List<ReviewInProfileViewModel>> GetAllForAthleteAsync(int athleteId);

        Task DeleteAsync(int clubId);

        Task<List<ReviewInListViewModel>> AllAsync(int page, int itemsPerPage = 8);

        int GetCount();
    }
}
