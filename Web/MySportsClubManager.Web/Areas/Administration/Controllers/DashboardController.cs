﻿namespace MySportsClubManager.Web.Areas.Administration.Controllers
{
    using System;
    using System.Security.Policy;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Common;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.Infrastructure.Common;
    using MySportsClubManager.Web.Infrastructure.Extensions;
    using static MySportsClubManager.Common.GlobalConstants;

    public class DashboardController : AdministrationController
    {
        private readonly IApplicationUserService applicationUserService;
        private readonly IAthleteService athleteService;
        private readonly ITrainerService trainerService;

        public DashboardController(IApplicationUserService applicationUserService, IAthleteService athleteService, ITrainerService trainerService)
        {
            this.applicationUserService = applicationUserService;
            this.athleteService = athleteService;
            this.trainerService = trainerService;
        }

        [HttpGet]
        public IActionResult Home()
        {
            return this.View();
        }

        [HttpGet]
        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> AllUsers()
        {
            var users = await this.applicationUserService.AllAsync();

            return this.View(users);
        }

        [HttpGet]
        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> CreateAdmin(string id)
        {
            try
            {
                await this.applicationUserService.AssignUserToRoleAsync(id, GlobalConstants.AdministratorRoleName);
                await this.trainerService.CreateAsync(id);
                this.TempData[GlobalConstants.SuccessMessage] = ExceptionMessages.SuccessRoleMessage;
                return this.RedirectToAction(nameof(this.AllUsers), "Dashboard");
            }
            catch (InvalidOperationException ioe)
            {
                this.TempData[GlobalConstants.ErrorMessage] = ioe.Message;
                return this.RedirectToAction(nameof(this.AllUsers), "Dashboard");
            }
        }

        [HttpGet]
        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> CreateTrainer(string id)
        {
            try
            {
                await this.applicationUserService.AssignUserToRoleAsync(id, GlobalConstants.TrainerRoleName);
                await this.trainerService.CreateAsync(id);
                this.TempData[GlobalConstants.SuccessMessage] = ExceptionMessages.SuccessRoleMessage;
                return this.RedirectToAction(nameof(this.AllUsers), "Dashboard");
            }
            catch (InvalidOperationException ioe)
            {
                this.TempData[GlobalConstants.ErrorMessage] = ioe.Message;
                return this.RedirectToAction(nameof(this.AllUsers), "Dashboard");
            }
        }

        [HttpGet]
        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> RemoveRole(string id)
        {
            try
            {
                if (id == this.User.Id())
                {
                    throw new InvalidOperationException(ExceptionMessages.DemoteYourselfErrorMessage);
                }

                await this.applicationUserService.RemoveUserFromRoleAsync(id);

                this.TempData[GlobalConstants.SuccessMessage] = ExceptionMessages.SuccessRemoveFromRoleMessage;
                return this.RedirectToAction(nameof(this.AllUsers), "Dashboard");
            }
            catch (InvalidOperationException ioe)
            {
                this.TempData[GlobalConstants.ErrorMessage] = ioe.Message;
                return this.RedirectToAction(nameof(this.AllUsers), "Dashboard");
            }
        }
    }
}
