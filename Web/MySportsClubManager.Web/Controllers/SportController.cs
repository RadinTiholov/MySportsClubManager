namespace MySportsClubManager.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Web.ViewModels.Sport;

    public class SportController : BaseController
    {
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateSportInputModel();

            return this.View(model);
        }
    }
}
