namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface ITrainerService
    {
        Task Create(string userId);
    }
}
