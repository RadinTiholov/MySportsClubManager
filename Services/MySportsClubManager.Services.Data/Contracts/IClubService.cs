﻿namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MySportsClubManager.Web.ViewModels.Club;

    public interface IClubService
    {
        Task Create(CreateClubInputModel model, string ownerId);

        Task<List<ClubViewModel>> AllAsync();
    }
}