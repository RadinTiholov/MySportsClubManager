namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using MySportsClubManager.Data.Models;

    public interface IImageService
    {
        Task<Image> Add(string imageUrl);
    }
}
