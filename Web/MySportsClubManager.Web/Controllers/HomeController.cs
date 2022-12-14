namespace MySportsClubManager.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.ViewModels;
    using MySportsClubManager.Web.ViewModels.Club;
    using MySportsClubManager.Web.ViewModels.Home;

    public class HomeController : BaseController
    {
        private readonly IClubService clubService;
        private readonly IMemoryCache memoryCache;

        public HomeController(IClubService clubService, IMemoryCache memoryCache)
        {
            this.clubService = clubService;
            this.memoryCache = memoryCache;
        }

        public async Task<IActionResult> Index()
        {
            if (!this.memoryCache.TryGetValue<List<ClubInListViewModel>>("RecommendedClubs", out var recommendedClubs))
            {
                recommendedClubs = await this.clubService.GetClubsWithHightestRating();
                this.memoryCache.Set("RecommendedClubs", recommendedClubs, TimeSpan.FromMinutes(5));
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
