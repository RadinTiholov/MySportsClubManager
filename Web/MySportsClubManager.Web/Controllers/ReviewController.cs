namespace MySportsClubManager.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.Infrastructure.Extensions;
    using MySportsClubManager.Web.ViewModels.Review;

    using static MySportsClubManager.Common.GlobalConstants;

    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : BaseController
    {
        private readonly IReviewService reviewService;
        private readonly IAthleteService athleteService;

        public ReviewController(IReviewService reviewService, IAthleteService athleteService)
        {
            this.reviewService = reviewService;
            this.athleteService = athleteService;
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult<CreateReviewResponseModel>> Create([FromBody] CreateReviewInputModel model)
        {
            try
            {
                var athleteId = await this.athleteService.GetAthleteIdAsync(this.User.Id());
                var createdReview = await this.reviewService.CreateAsync(this.User.Id(), athleteId, model);
                var avarageRating = this.reviewService.GetAverageForClub(model.ClubId);
                return this.Json(new CreateReviewResponseModel()
                {
                    AvarageRating = avarageRating,
                    Review = createdReview,
                });
            }
            catch (Exception)
            {
                return this.BadRequest();
            }
        }
    }
}
