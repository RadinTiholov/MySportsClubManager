namespace MySportsClubManager.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.ViewModels.Club;
    using MySportsClubManager.Web.ViewModels.Training;

    public class TrainingController : BaseController
    {
        private readonly ITrainingService trainingService;
        private readonly IClubService clubService;

        public TrainingController(ITrainingService trainingService, IClubService clubService)
        {
            this.trainingService = trainingService;
            this.clubService = clubService;
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
    }
}
