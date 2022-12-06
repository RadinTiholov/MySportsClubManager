namespace MySportsClubManager.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Common;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.Infrastructure.Common;
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
                this.TempData[GlobalConstants.SuccessMessage] = ExceptionMessages.SuccessfullyAddedMessage;
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
                try
                {
                    await this.contestService.DeleteAsync(id);

                    this.TempData[GlobalConstants.SuccessMessage] = ExceptionMessages.SuccessfullyDeletedMessage;
                    return this.RedirectToAction("All", "Contest", new { area = string.Empty });
                }
                catch (Exception)
                {
                    return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 404, area = string.Empty });
                }
            }

            return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 401, area = string.Empty });
        }

        [HttpGet]
        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await this.contestService.GetOneAsync<EditContestViewModel>(id);
            model.Sports = await this.sportsService.AllForInputAsync();

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> Edit(EditContestViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Sports = await this.sportsService.AllForInputAsync();
                return this.View(model);
            }

            try
            {
                await this.contestService.EditAsync(model);

                this.TempData[GlobalConstants.SuccessMessage] = ExceptionMessages.SuccessfullyEditedMessage;
                return this.RedirectToAction("Details", "Contest", new { id = model.Id, area = string.Empty });
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
        public async Task<IActionResult> ChooseWinners(int contestId)
        {
            try
            {
                var model = new ChooseWinnerInputModel();
                model.Athletes = await this.contestService.GetAllParticipantsAsync(contestId);
                model.ContestId = contestId;
                return this.View(model);
            }
            catch (Exception)
            {
                return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 404, area = string.Empty });
            }
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> ChooseWinners(ChooseWinnerInputModel model)
        {
            if (!this.ModelState.IsValid || model.FirsPlaceId == model.SecondPlaceId || model.FirsPlaceId == model.ThirdPlaceId || model.SecondPlaceId == model.ThirdPlaceId)
            {
                model.Athletes = await this.contestService.GetAllParticipantsAsync(model.ContestId);
                model.ContestId = model.ContestId;
                this.ModelState.AddModelError(string.Empty, ExceptionMessages.WinnersErrorMessage);
                return this.View(model);
            }

            try
            {
                await this.contestService.SetWinnersAsync(model.ContestId, model.FirsPlaceId, model.SecondPlaceId, model.ThirdPlaceId);

                this.TempData[GlobalConstants.SuccessMessage] = ExceptionMessages.SuccessfullyEditedMessage;
                return this.RedirectToAction("Details", "Contest", new { id = model.ContestId, area = string.Empty });
            }
            catch (Exception)
            {
                return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 401, area = string.Empty });
            }
        }
    }
}
