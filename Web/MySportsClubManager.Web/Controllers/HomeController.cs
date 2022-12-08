namespace MySportsClubManager.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.ViewModels;
    using MySportsClubManager.Web.ViewModels.Home;

    public class HomeController : BaseController
    {
        private readonly IClubService clubService;

        public HomeController(IClubService clubService)
        {
            this.clubService = clubService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel()
            {
                RecommendedClubs = await this.clubService.GetClubsWithHightestRating(),
            };

            return this.View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        public IActionResult ErrorStatus(int statusCode)
        {
            return this.View(statusCode);
        }
    }
}
