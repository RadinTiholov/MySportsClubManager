namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using MySportsClubManager.Data.Models;

    public interface IImageService
    {
        Task<Image> AddByFile(IFormFile imageFile, string name);

        Task<Image> AddByUrlAsync(string Url);
    }
}
