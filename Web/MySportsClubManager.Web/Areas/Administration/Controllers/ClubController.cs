namespace MySportsClubManager.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.Infrastructure.Extensions;
    using MySportsClubManager.Web.ViewModels.Club;

    using static MySportsClubManager.Common.GlobalConstants;

    public class ClubController : AdministrationController
    {
        private readonly IClubService clubService;
        private readonly ISportService sportsService;

        public ClubController(IClubService clubService, ISportService sportsService)
        {
            this.clubService = clubService;
            this.sportsService = sportsService;
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
                //string userId = this.User.Id();
                //ToDo get triner Id from user
                await this.clubService.Create(model, 0);

                return this.RedirectToAction("All", "Club", new { area = string.Empty });
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, CreationErrorMessage);

                model.Sports = await this.sportsService.AllForInputAsync();
                return this.View(model);
            }
        }
    }
}
