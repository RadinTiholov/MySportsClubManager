namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MySportsClubManager.Services.Data.Contracts.Base;
    using MySportsClubManager.Web.ViewModels.Athlete;
    using MySportsClubManager.Web.ViewModels.Contest;

    public interface IContestService : IPaginationBase
    {
        Task CreateAsync(CreateContestViewModel model);

        Task<T> GetOneAsync<T>(int id);

        Task DeleteAsync(int contestId);

        Task EditAsync(EditContestViewModel model);

        Task Register(int contestId, string userId);
    }
}
