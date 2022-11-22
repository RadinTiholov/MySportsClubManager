namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using MySportsClubManager.Services.Data.Contracts.Base;
    using MySportsClubManager.Web.ViewModels.Contest;

    public interface IContestService : IPaginationBase
    {
        Task CreateAsync(CreateContestViewModel model);

        Task<T> GetOneAsync<T>(int id);

        Task DeleteAsync(int contestId);
    }
}
