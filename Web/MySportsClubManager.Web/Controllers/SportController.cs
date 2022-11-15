namespace MySportsClubManager.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.ViewModels.Sport;

    public class SportController : BaseController
    {
        private readonly ISportService sportService;

        public SportController(ISportService sportService)
        {
            this.sportService = sportService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> All(int id = 1)
        {
            if (id < 1)
            {
                id = 1;
            }

            const int ItemsPerPage = 8;
            var model = new SportListViewModel()
            {
                ItemsPerPage = ItemsPerPage,
                Sports = await this.sportService.AllAsync<SportInListViewModel>(id, ItemsPerPage),
                PageNumber = id,
                SportsCount = this.sportService.GetCount(),
            };

            return this.View(model);
        }
    }
}
