namespace MySportsClubManager.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Caching.Memory;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.Infrastructure.Extensions;
    using MySportsClubManager.Web.ViewModels;
    using MySportsClubManager.Web.ViewModels.Club;
    using MySportsClubManager.Web.ViewModels.Home;
    using static Microsoft.ML.ForecastingCatalog;

    public class HomeController : BaseController
    {
        private readonly IClubService clubService;
        private readonly IDistributedCache cache;

        public HomeController(IClubService clubService, IDistributedCache cache)
        {
            this.clubService = clubService;
            this.cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            string recordKey = "MySportsClubManager_" + DateTime.Now.ToString("yyyyMMdd_hh");

            var recommendedClubs = await this.cache.GetRecordAsync<List<ClubInListViewModel>>(recordKey);

            if (recommendedClubs is null)
            {
                recommendedClubs = await this.clubService.GetClubsWithHightestRating();

                await this.cache.SetRecordAsync(recordKey, recommendedClubs, TimeSpan.FromHours(1));
            }

            var model = new HomeViewModel()
            {
                RecommendedClubs = recommendedClubs,
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
