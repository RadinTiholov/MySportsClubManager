namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface ITrainerService
    {
        Task Create(string userId);

        Task<int> GetTrainerId(string userId);

        Task<bool> OwnsClub(string userId, int clubId);
    }
}
