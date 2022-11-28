namespace MySportsClubManager.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Common;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.Infrastructure.Common;
    using MySportsClubManager.Web.Infrastructure.Extensions;
    using MySportsClubManager.Web.ViewModels.Club;
    using MySportsClubManager.Web.ViewModels.Training;

    using static MySportsClubManager.Common.GlobalConstants;

    public class TrainingController : AdministrationController
    {
        private readonly IClubService clubService;
        private readonly ITrainerService trainerService;
        private readonly ITrainingService trainingService;

        public TrainingController(IClubService clubServicel, ITrainerService trainerService, ITrainingService trainingService)
        {
            this.clubService = clubServicel;
            this.trainerService = trainerService;
            this.trainingService = trainingService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateTrainingInputModel();
            int trainerId = await this.trainerService.GetTrainerIdAsync(this.User.Id());
            model.Clubs = await this.clubService.GetMineAsync<ClubsInDropdownViewModel>(trainerId);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainingInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                int trainerId = await this.trainerService.GetTrainerIdAsync(this.User.Id());
                model.Clubs = await this.clubService.GetMineAsync<ClubsInDropdownViewModel>(trainerId);

                return this.View(model);
            }

            try
            {
                await this.trainingService.CreateAsync(model);

                this.TempData[GlobalConstants.SuccessMessage] = ExceptionMessages.SuccessfullyAddedMessage;
                return this.RedirectToAction("All", "Training", new { area = string.Empty, clubId = model.ClubId });
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, CreationErrorMessage);

                return this.View(model);
            }
        }

        public async Task<IActionResult> Delete(int trainingId, int clubId)
        {
            if (this.User.IsInRole(AdministratorRoleName) || await this.trainerService.OwnsClub(this.User.Id(), clubId))
            {
                await this.trainingService.DeleteAsync(trainingId);
                this.TempData[GlobalConstants.SuccessMessage] = ExceptionMessages.SuccessfullyDeletedMessage;
                return this.RedirectToAction("All", "Training", new { area = string.Empty, clubId = clubId });
            }

            return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 401, area = string.Empty });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (this.User.IsInRole(AdministratorRoleName) || await this.trainerService.OwnsClub(this.User.Id(), id))
            {
                var model = await this.trainingService.GetOneAsync<EditTrainingInputModel>(id);
                model.Clubs = await this.clubService.GetMineAsync<ClubsInDropdownViewModel>(model.TrainerId);

                return this.View(model);
            }

            return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 401, area = string.Empty });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTrainingInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Clubs = await this.clubService.GetMineAsync<ClubsInDropdownViewModel>(model.TrainerId);
                return this.View(model);
            }

            if (this.User.IsInRole(AdministratorRoleName) || await this.trainerService.OwnsClub(this.User.Id(), model.Id))
            {
                try
                {
                    await this.trainingService.EditAsync(model);

                    this.TempData[GlobalConstants.SuccessMessage] = ExceptionMessages.SuccessfullyEditedMessage;
                    return this.RedirectToAction("All", "Training", new { area = string.Empty, clubId = model.ClubId });
                }
                catch (Exception)
                {
                    this.ModelState.AddModelError(string.Empty, CreationErrorMessage);
                    model.Clubs = await this.clubService.GetMineAsync<ClubsInDropdownViewModel>(model.Id);

                    return this.View(model);
                }
            }

            return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 401, area = string.Empty });
        }
    }
}
