namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MySportsClubManager.Web.ViewModels.Athlete;
    using MySportsClubManager.Web.ViewModels.Win;

    public interface IAthleteService
    {
        Task CreateAsync(string userId);

        Task RegisterSportClubAsync(string userId, int clubId);

        Task UnregisterSportClubAsync(string userId, int clubId);

        Task<bool> IsEnrolledInClubAsync(string userId, int clubId);

        Task<bool> IsEnrolledInTrainingAsync(string userId, int trainingId);

        Task<bool> IsEnrolledInAnyClubAsync(string userId);

        Task<int> GetMyClub(string userId);

        Task<int> GetAthleteIdAsync(string userId);

        Task<List<AthleteInListViewModel>> GetAllForContestAsync(int clubId);

        Task<List<AchievementInListViewModel>> GetAllAchievementsForAthleteAsync(int athleteId);
    }
}
