namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using MySportsClubManager.Web.ViewModels.Contest;

    public interface IContestService
    {
        Task CreateAsync(CreateContestViewModel model);
    }
}
