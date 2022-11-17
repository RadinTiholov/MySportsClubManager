namespace MySportsClubManager.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.Infrastructure.Extensions;
    using MySportsClubManager.Web.ViewModels.Club;

    using static MySportsClubManager.Common.GlobalConstants;

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
                int trainerId = await this.trainerService.GetTrainerId(this.User.Id());
                await this.clubService.Create(model, trainerId);

                return this.RedirectToAction("All", "Club", new { area = string.Empty });
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, CreationErrorMessage);

                model.Sports = await this.sportsService.AllForInputAsync();
                return this.View(model);
            }
        }

        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> Delete(int clubId)
        {
            await this.clubService.Delete(clubId);
            return this.RedirectToAction("All", "Club", new { area = string.Empty });
        }
    }
}
