namespace MySportsClubManager.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Web.ViewModels.ApplicationUser;

    [Authorize]
    public class ApplicationUserController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterViewModel();

            return View(model);
        }
    }
}
