namespace MySportsClubManager.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.ViewModels.Club;

    public class ClubController : BaseController
    {
        private readonly ISportService sportService;

        public ClubController(ISportService sportService)
        {
            this.sportService = sportService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateClubInputModel();
            var sports = await this.sportService.AllForInputAsync();
            model.Sports = sports;

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateClubInputModel model)
        {
            return this.Ok();
        }
    }
}
