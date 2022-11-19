namespace MySportsClubManager.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.ViewModels.Club;


    public class ClubController : BaseController
    {
        private readonly IClubService clubService;

        public ClubController(IClubService clubService)
        {
            this.clubService = clubService;
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
            var model = new ClubListViewModel()
            {
                ItemsPerPage = ItemsPerPage,
                Clubs = await this.clubService.AllAsync<ClubInListViewModel>(id, ItemsPerPage),
                PageNumber = id,
                SportsCount = this.clubService.GetCount(),
            };

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var model = await this.clubService.GetOne(id);
                return this.View(model);
            }
            catch (ArgumentException)
            {
                //ToDo 404 page redirection
                throw;
            }
        }
    }
}
