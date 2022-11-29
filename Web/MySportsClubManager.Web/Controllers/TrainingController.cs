namespace MySportsClubManager.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Common;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.Infrastructure.Common;
    using MySportsClubManager.Web.Infrastructure.Extensions;
    using MySportsClubManager.Web.ViewModels.Club;
    using MySportsClubManager.Web.ViewModels.Training;

    public class TrainingController : BaseController
    {
        private readonly ITrainingService trainingService;
        private readonly IClubService clubService;
        private readonly IAthleteService athleteService;

        public TrainingController(ITrainingService trainingService, IClubService clubService, IAthleteService athleteService)
        {
            this.trainingService = trainingService;
            this.clubService = clubService;
            this.athleteService = athleteService;
        }

        public async Task<IActionResult> All(int clubId)
        {
            try
            {
                var club = await this.clubService.GetOneAsync<ClubDetailsViewModel>(clubId);
                var model = new TrainingListViewModel()
                {
                    Trainings = await this.trainingService.GetAllForClubAsync(clubId),
                    Club = club,
                };
                return this.View(model);
            }
            catch (Exception)
            {
                return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 404 });
            }
        }

        public async Task<IActionResult> Enroll(int trainingId)
        {
            int? clubId = null;
            try
            {
                clubId = await this.trainingService.GetClubId(trainingId);
                if (!await this.athleteService.IsEnrolledInClubAsync(this.User.Id(), (int)clubId))
                {
                    throw new ArgumentNullException();
                }

                await this.trainingService.Enroll(trainingId, this.User.Id());
                this.TempData[GlobalConstants.SuccessMessage] = ExceptionMessages.SuccessfullyEnrolledMessage;

                return this.RedirectToAction(nameof(this.All), new { clubId = clubId });
            }
            catch (ArgumentNullException)
            {
                return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 404 });
            }
            catch (ArgumentException)
            {
                this.TempData[GlobalConstants.WarningMessage] = ExceptionMessages.AlreadyEnrolledMessage;
                return this.RedirectToAction(nameof(this.All), new { clubId = clubId });
            }
        }
    }
}
