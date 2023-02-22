namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Web.ViewModels.ApplicationUser;

    public interface IApplicationUserService
    {
        Task<List<ApplicationUserInformationViewModel>> AllAsync();

        Task AssignUserToRoleAsync(string id, string roleName);

        Task RemoveUserFromRoleAsync(string id);

        Task<string> GetCurrentUserProfilePicAsync(string id);

        Task<ProfileViewModel> GetProfileInformationAsync(string id);

        Task<ApplicationUser> FindByNameAsync(string username);
    }
}
