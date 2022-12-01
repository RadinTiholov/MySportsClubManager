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

    [Authorize]
    public class ClubController : BaseController
    {
        private readonly IClubService clubService;
        private readonly IAthleteService athleteService;
        private readonly IReviewService reviewService;

        public ClubController(IClubService clubService, IAthleteService athleteService, IReviewService reviewService)
        {
            this.clubService = clubService;
            this.athleteService = athleteService;
            this.reviewService = reviewService;
        }

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
                model.Reviews = await this.reviewService.AllAsync();
                model.AvarageRating = this.reviewService.GetAverageForClub(id);
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
                this.TempData[GlobalConstants.SuccessMessage] = ExceptionMessages.SuccessfullyEnrolledMessage;

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

        [HttpGet]
        public async Task<IActionResult> Disenroll(int clubId)
        {
            try
            {
                await this.clubService.Disenroll(clubId, this.User.Id());
                this.TempData[GlobalConstants.SuccessMessage] = ExceptionMessages.SuccessfullyDisenrolledMessage;

                return this.RedirectToAction("Details", "Club", new { id = clubId });
            }
            catch (ArgumentNullException)
            {
                return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 404 });
            }
            catch (ArgumentException)
            {
                this.TempData[GlobalConstants.WarningMessage] = ExceptionMessages.NotEnrolledMessage;
                return this.RedirectToAction("Details", "Club", new { id = clubId });
            }
        }

        [HttpGet]
        public async Task<IActionResult> MyClub()
        {
            try
            {
                var clubId = await this.athleteService.GetMyClub(this.User.Id());

                return this.RedirectToAction("Details", "Club", new { id = clubId });
            }
            catch (ArgumentNullException)
            {
                this.TempData[GlobalConstants.WarningMessage] = ExceptionMessages.PleaseEnrollInClubFirstMessage;
                return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 404 });
            }
        }
    }
}
