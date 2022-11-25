namespace MySportsClubManager.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Services.Data.Contracts;

    public class TrainerController : BaseController
    {
        private readonly ITrainerService trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            this.trainerService = trainerService;
        }

        public async Task<IActionResult> Contact(int id)
        {
            var model = await this.trainerService.GetTrainerInformationAsync(id);
            return this.View(model);
        }
    }
}
