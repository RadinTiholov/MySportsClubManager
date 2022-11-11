namespace MySportsClubManager.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Services.Data.Contracts;

    public class DashboardController : AdministrationController
    {
        private readonly IApplicationUserService applicationUserService;

        public DashboardController(IApplicationUserService applicationUserService)
        {
            this.applicationUserService = applicationUserService;
        }

        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            var users = await this.applicationUserService.AllAsync();

            return this.View(users);
        }
    }
}
