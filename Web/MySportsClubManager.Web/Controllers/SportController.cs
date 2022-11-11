namespace MySportsClubManager.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.ViewModels.Sport;

    using static MySportsClubManager.Common.GlobalConstants;

    public class SportController : BaseController
    {
        private readonly ISportService sportService;

        public SportController(ISportService sportService)
        {
            this.sportService = sportService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateSportInputModel();

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateSportInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                await this.sportService.Create(model);

                return this.RedirectToAction("All", "Sport");
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, CreationErrorMessage);

                return this.View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var sports = await this.sportService.AllAsync();

            return this.View(sports);
        }
    }
}
