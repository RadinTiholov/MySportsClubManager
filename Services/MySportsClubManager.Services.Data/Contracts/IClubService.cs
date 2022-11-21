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
    }
}
