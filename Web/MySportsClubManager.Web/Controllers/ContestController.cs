namespace MySportsClubManager.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Common;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.Infrastructure.Common;
    using MySportsClubManager.Web.Infrastructure.Extensions;
    using MySportsClubManager.Web.ViewModels.Contest;

    [Authorize]
    public class ContestController : BaseController
    {
        private readonly IContestService contestService;
        private readonly ISportService sportService;
        private readonly IAthleteService athleteService;

        public ContestController(IContestService contestService, ISportService sportService, IAthleteService athleteService)
        {
            this.contestService = contestService;
            this.sportService = sportService;
            this.athleteService = athleteService;
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

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var model = await this.contestService.GetOneAsync<ContestDetailsViewModel>(id);
                model.Athletes = await this.athleteService.GetAllForContestAsync(id);
                model.Champions = model.Champions.OrderBy(x => x.Place).ToList();
                return this.View(model);
            }
            catch (System.Exception)
            {
                return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 404 });
            }
        }

        public async Task<IActionResult> Register(int contestId)
        {
            try
            {
                await this.contestService.Register(contestId, this.User.Id());
                this.TempData[GlobalConstants.SuccessMessage] = ExceptionMessages.SuccessfullyEnrolledMessage;

                return this.RedirectToAction("Details", "Contest", new { id = contestId });
            }
            catch (ArgumentNullException)
            {
                return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 404 });
            }
            catch (ArgumentException ae)
            {
                this.TempData[GlobalConstants.WarningMessage] = ae.Message;
                return this.RedirectToAction("Details", "Contest", new { id = contestId });
            }
        }
    }
}
