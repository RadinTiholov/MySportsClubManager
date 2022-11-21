namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface ITrainerService
    {
        Task CreateAsync(string userId);

        Task<int> GetTrainerIdAsync(string userId);

        Task<bool> OwnsClub(string userId, int clubId);
    }
}
