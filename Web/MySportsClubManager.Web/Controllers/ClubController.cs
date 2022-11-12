namespace MySportsClubManager.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Common;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.Infrastructure.Extensions;
    using MySportsClubManager.Web.ViewModels.Club;

    using static MySportsClubManager.Common.GlobalConstants;

    public class ClubController : BaseController
    {
        private readonly IClubService clubService;
        private readonly ISportService sportsService;

        public ClubController(IClubService clubService, ISportService sportsService)
        {
            this.clubService = clubService;
            this.sportsService = sportsService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var club = await this.clubService.AllAsync();

            return this.View(club);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateClubInputModel();
            model.Sports = await this.sportsService.AllForInputAsync();

            return this.View(model);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
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
                string userId = this.User.Id();
                await this.clubService.Create(model, userId);

                return this.RedirectToAction("Index", "Home");
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
