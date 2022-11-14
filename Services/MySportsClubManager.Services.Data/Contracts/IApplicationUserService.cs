﻿namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MySportsClubManager.Web.ViewModels.ApplicationUser;

    public interface IApplicationUserService
    {
        Task<List<ApplicationUserInformationViewModel>> AllAsync();

        Task AssignUserToRole(string id, string roleName);

        Task<string> GetCurrentUserProfilePic(string id);
    }
}
