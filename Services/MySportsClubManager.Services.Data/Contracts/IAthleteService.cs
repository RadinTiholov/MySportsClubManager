namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IAthleteService
    {
        Task CreateAsync(string userId);

        Task RegisterSportClubAsync(string userId, int clubId);

        Task UnregisterSportClubAsync(string userId, int clubId);

        Task<bool> IsEnrolledInClubAsync(string userId, int clubId);

        Task<bool> IsEnrolledInAnyClubAsync(string userId);
    }
}
