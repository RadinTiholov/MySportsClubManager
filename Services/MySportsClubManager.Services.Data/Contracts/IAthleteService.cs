namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IAthleteService
    {
        Task Create(string userId);
    }
}
