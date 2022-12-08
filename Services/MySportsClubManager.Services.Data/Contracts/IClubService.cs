namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MySportsClubManager.Services.Data.Contracts.Base;
    using MySportsClubManager.Web.ViewModels.Club;

    public interface IClubService : IPaginationBase
    {
        Task CreateAsync(CreateClubInputModel model, int trainerId);

        Task DeleteAsync(int clubId);

        Task<T> GetOneAsync<T>(int id);

        Task EditAsync(EditClubInputModel model);

        Task Enroll(int clubId, string userId);

        Task Disenroll(int clubId, string userId);

        Task<List<T>> AllCreatedAsync<T>(int page, int trainerId, int itemsPerPage = 8);

        Task<List<T>> GetMineAsync<T>(int trainerId);

        Task<List<ClubInListViewModel>> GetAllForSearchAsync(string searchQuery);

        Task<List<ClubInListViewModel>> GetClubsWithSameSportAsync(string sportName);
    }
}
