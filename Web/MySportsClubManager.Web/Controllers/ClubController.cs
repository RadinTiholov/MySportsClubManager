namespace MySportsClubManager.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Common;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.Infrastructure.Common;
    using MySportsClubManager.Web.Infrastructure.Extensions;
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
                var model = await this.clubService.GetOneAsync<ClubDetailsViewModel>(id);
                return this.View(model);
            }
            catch (ArgumentException)
            {
                return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 404 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Enroll(int clubId)
        {
            try
            {
                await this.clubService.Enroll(clubId, this.User.Id());
                this.TempData[GlobalConstants.SuccessMessage] = ExceptionMessages.SuccessfullyAddedMessage;

                return this.RedirectToAction("Details", "Club", new { id = clubId });
            }
            catch (ArgumentNullException)
            {
                return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 404 });
            }
            catch (ArgumentException)
            {
                this.TempData[GlobalConstants.WarningMessage] = ExceptionMessages.AlreadyEnrolledMessage;
                return this.RedirectToAction("Details", "Club", new { id = clubId });
            }
        }
    }
}
