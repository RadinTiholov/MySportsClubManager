namespace MySportsClubManager.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.ViewModels.Sport;

    using static MySportsClubManager.Common.GlobalConstants;

    public class SportController : AdministrationController
    {
        private readonly ISportService sportService;

        public SportController(ISportService sportService)
        {
            this.sportService = sportService;
        }

        [HttpGet]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Create()
        {
            var model = new CreateSportInputModel();

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> Create(CreateSportInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                await this.sportService.Create(model);

                return this.RedirectToAction("All", "Sport", new { area = string.Empty });
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, CreationErrorMessage);

                return this.View(model);
            }
        }

        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> Delete(int sportId)
        {
            await this.sportService.Delete(sportId);
            return this.RedirectToAction("All", "Sport", new { area = string.Empty });
        }
    }
}
