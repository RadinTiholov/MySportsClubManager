namespace MySportsClubManager.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization.Infrastructure;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using MySportsClubManager.Common;
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

        [HttpGet]
        public async Task<IActionResult> CreateAdmin(string id)
        {
            try
            {
                await this.applicationUserService.AssignUserToRole(id, GlobalConstants.AdministratorRoleName);
                return this.RedirectToAction(nameof(this.AllUsers), "Dashboard");
            }
            catch (InvalidOperationException ioe)
            {
                this.TempData[GlobalConstants.ErrorMessage] = ioe.Message;
                return this.RedirectToAction(nameof(this.AllUsers), "Dashboard");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateTrainer(string id)
        {
            return this.RedirectToAction(nameof(this.AllUsers), "Dashboard");
        }
    }
}
