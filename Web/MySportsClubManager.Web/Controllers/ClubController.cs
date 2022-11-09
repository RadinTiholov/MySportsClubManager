namespace MySportsClubManager.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Services.Data.Contracts;
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
        public async Task<IActionResult> Create()
        {
            var model = new CreateClubInputModel();
            model.Sports = await this.sportsService.AllForInputAsync();

            return this.View(model);
        }

        [Authorize]
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
                var userId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
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
