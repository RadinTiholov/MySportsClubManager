namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IAthleteService
    {
        Task CreateAsync(string userId);
    }
}
