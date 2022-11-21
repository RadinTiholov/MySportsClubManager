﻿namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Http;

    public interface ICloudinaryService
    {
        Task<string> UploadAsync(IFormFile file, string name);

        Task DeleteImageAsync(Cloudinary cloudinary, string name);
    }
}
