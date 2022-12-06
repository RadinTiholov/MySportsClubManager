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

    using static MySportsClubManager.Common.GlobalConstants;
    using static MySportsClubManager.Web.Infrastructure.Common.ExceptionMessages;

    public class ClubController : AdministrationController
    {
        private readonly IClubService clubService;
        private readonly ISportService sportsService;
        private readonly ITrainerService trainerService;

        public ClubController(IClubService clubService, ISportService sportsService, ITrainerService trainerService)
        {
            this.clubService = clubService;
            this.sportsService = sportsService;
            this.trainerService = trainerService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateClubInputModel();
            model.Sports = await this.sportsService.AllForInputAsync();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Sports = await this.sportsService.AllForInputAsync();
                return this.View(model);
            }

            try
            {
                int trainerId = await this.trainerService.GetTrainerIdAsync(this.User.Id());
                await this.clubService.CreateAsync(model, trainerId);

                this.TempData[GlobalConstants.SuccessMessage] = SuccessfullyAddedMessage;
                return this.RedirectToAction("All", "Club", new { area = string.Empty });
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);

                model.Sports = await this.sportsService.AllForInputAsync();
                return this.View(model);
            }
        }

        public async Task<IActionResult> Delete(int clubId)
        {
            if (this.User.IsInRole(AdministratorRoleName) || await this.trainerService.OwnsClubAsync(this.User.Id(), clubId))
            {
                try
                {
                    await this.clubService.DeleteAsync(clubId);
                    this.TempData[GlobalConstants.SuccessMessage] = ExceptionMessages.SuccessfullyDeletedMessage;
                    return this.RedirectToAction("All", "Club", new { area = string.Empty });
                }
                catch (Exception)
                {
                    return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 404, area = string.Empty });
                }
            }

            return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 401, area = string.Empty });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (this.User.IsInRole(AdministratorRoleName) || await this.trainerService.OwnsClubAsync(this.User.Id(), id))
            {
                var model = await this.clubService.GetOneAsync<EditClubInputModel>(id);
                model.Sports = await this.sportsService.AllForInputAsync();

                return this.View(model);
            }

            return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 401, area = string.Empty });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditClubInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Sports = await this.sportsService.AllForInputAsync();
                return this.View(model);
            }

            if (this.User.IsInRole(AdministratorRoleName) || await this.trainerService.OwnsClubAsync(this.User.Id(), model.Id))
            {
                try
                {
                    await this.clubService.EditAsync(model);

                    this.TempData[GlobalConstants.SuccessMessage] = ExceptionMessages.SuccessfullyEditedMessage;
                    return this.RedirectToAction("Details", "Club", new { id = model.Id, area = string.Empty });
                }
                catch (Exception)
                {
                    this.ModelState.AddModelError(string.Empty, CreationErrorMessage);
                    model.Sports = await this.sportsService.AllForInputAsync();

                    return this.View(model);
                }
            }

            return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 401, area = string.Empty });
        }

        [HttpGet]
        public async Task<IActionResult> CreatedClubs(int id = 1)
        {
            if (id < 1)
            {
                id = 1;
            }

            int trainerId = await this.trainerService.GetTrainerIdAsync(this.User.Id());
            const int ItemsPerPage = 8;
            var model = new ClubListViewModel()
            {
                ItemsPerPage = ItemsPerPage,
                Clubs = await this.clubService.AllCreatedAsync<ClubInListViewModel>(id, trainerId, ItemsPerPage),
                PageNumber = id,
                Count = this.clubService.GetCount(),
            };

            return this.View(model);
        }
    }
}
