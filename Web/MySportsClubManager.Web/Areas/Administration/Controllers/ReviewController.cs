﻿namespace MySportsClubManager.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Common;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.Infrastructure.Common;
    using MySportsClubManager.Web.ViewModels.Review;
    using MySportsClubManager.Web.ViewModels.Sport;

    public class ReviewController : AdministrationController
    {
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> All(int id = 1)
        {
            if (id < 1)
            {
                id = 1;
            }

            const int ItemsPerPage = 8;
            var model = new ReviewListViewModel()
            {
                ItemsPerPage = ItemsPerPage,
                Reviews = await this.reviewService.AllAsync(id, ItemsPerPage),
                PageNumber = id,
                Count = this.reviewService.GetCount(),
            };

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int reviewId)
        {
            try
            {
                await this.reviewService.DeleteAsync(reviewId);
                this.TempData[GlobalConstants.SuccessMessage] = ExceptionMessages.SuccessfullyDeletedMessage;
                return this.RedirectToAction("All", "Review", new { area = "Administration" });
            }
            catch (Exception)
            {
                return this.RedirectToAction("ErrorStatus", "Home", new { statusCode = 404 });
            }
        }
    }
}
