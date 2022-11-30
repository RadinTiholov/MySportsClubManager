namespace MySportsClubManager.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Common;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.Infrastructure.Common;
    using MySportsClubManager.Web.ViewModels.Trainer;

    [Authorize]
    public class TrainerController : BaseController
    {
        private readonly ITrainerService trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            this.trainerService = trainerService;
        }

        [HttpGet]
        public async Task<IActionResult> Contact(int id)
        {
            var model = await this.trainerService.GetTrainerInformationAsync(id);
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactTrainerInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                await this.trainerService.ContactWithTrainerAsync(model);

                this.TempData[GlobalConstants.SuccessMessage] = ExceptionMessages.SuccessfullySendEmailMessage;
                return this.RedirectToAction("All", "Club", new { area = string.Empty });
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, GlobalConstants.CreationErrorMessage);

                return this.View(model);
            }
        }
    }
}
