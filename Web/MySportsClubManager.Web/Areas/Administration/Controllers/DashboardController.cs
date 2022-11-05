namespace MySportsClubManager.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Web.ViewModels.Administration.Dashboard;

    public class DashboardController : AdministrationController
    {

        public IActionResult Index()
        {
            return this.View();
        }
    }
}
