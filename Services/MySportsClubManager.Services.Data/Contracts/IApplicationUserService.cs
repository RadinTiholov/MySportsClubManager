namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MySportsClubManager.Web.ViewModels.ApplicationUser;

    public interface IApplicationUserService
    {
        Task<List<ApplicationUserInformationViewModel>> AllAsync();
    }
}
