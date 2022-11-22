namespace MySportsClubManager.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Common;
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
                await this.sportService.CreateAsync(model);

                this.TempData[GlobalConstants.SuccessMessage] = GlobalConstants.SuccessfullyAddedMessage;
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
            await this.sportService.DeleteAsync(sportId);
            this.TempData[GlobalConstants.SuccessMessage] = GlobalConstants.SuccessfullyDeletedMessage;
            return this.RedirectToAction("All", "Sport", new { area = string.Empty });
        }

        [Authorize(Roles = AdministratorRoleName)]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await this.sportService.GetOneAsync<EditSportInputModel>(id);
                return this.View(model);
            }
            catch (ArgumentException)
            {
                return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 404 });
            }
        }

        [Authorize(Roles = AdministratorRoleName)]
        [HttpPost]
        public async Task<IActionResult> Edit(EditSportInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                await this.sportService.EditAsync(model);

                this.TempData[GlobalConstants.SuccessMessage] = GlobalConstants.SuccessfullyEditedMessage;
                return this.RedirectToAction("Details", "Sport", new { id = model.Id, area = string.Empty });
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, CreationErrorMessage);

                return this.View(model);
            }
        }
    }
}
