﻿namespace MySportsClubManager.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.ViewModels.Contest;

    public class ContestController : Controller
    {
        private readonly IContestService contestService;
        private readonly ISportService sportService;

        public ContestController(IContestService contestService, ISportService sportService)
        {
            this.contestService = contestService;
            this.sportService = sportService;
        }

        public async Task<IActionResult> All(int id)
        {
            if (id < 1)
            {
                id = 1;
            }

            const int ItemsPerPage = 8;
            var model = new ContestListViewModel()
            {
                ItemsPerPage = ItemsPerPage,
                Contest = await this.contestService.AllAsync<ContestInListViewModel>(id, ItemsPerPage),
                PageNumber = id,
                SportsCount = this.contestService.GetCount(),
                RecentSports = await this.sportService.GetRecent(),
            };

            return this.View(model);
        }
    }
}