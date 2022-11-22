namespace MySportsClubManager.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.ViewModels.Contest;

    using static MySportsClubManager.Common.GlobalConstants;

    public class ContestController : AdministrationController
    {
        private readonly ISportService sportsService;
        private readonly IContestService contestService;

        public ContestController(ISportService sportsService, IContestService contestService)
        {
            this.sportsService = sportsService;
            this.contestService = contestService;
        }

        [HttpGet]
        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> Create()
        {
            var model = new CreateContestViewModel();
            model.Sports = await this.sportsService.AllForInputAsync();

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> Create(CreateContestViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Sports = await this.sportsService.AllForInputAsync();
                return this.View(model);
            }

            try
            {
                await this.contestService.CreateAsync(model);
                return this.RedirectToAction("All", "Contest", new { area = string.Empty });
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, CreationErrorMessage);

                model.Sports = await this.sportsService.AllForInputAsync();
                return this.View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> Delete(int id)
        {
            if (this.User.IsInRole(AdministratorRoleName))
            {
                await this.contestService.DeleteAsync(id);
                return this.RedirectToAction("All", "Contest", new { area = string.Empty });
            }

            return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 401, area = string.Empty });
        }
    }
}
