namespace MySportsClubManager.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ML.Model;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.Review;

    public class ReviewService : IReviewService
    {
        private readonly IDeletableEntityRepository<Review> reviewRepository;
        private readonly IRepository<Club> clubRepository;
        private readonly IRepository<ApplicationUser> applicationUserRepository;

        public ReviewService(IDeletableEntityRepository<Review> reviewRepository, IRepository<Club> clubRepository, IRepository<ApplicationUser> applicationUserRepository)
        {
            this.reviewRepository = reviewRepository;
            this.clubRepository = clubRepository;
            this.applicationUserRepository = applicationUserRepository;
        }

        public async Task<List<ReviewInListViewModel>> AllAsync(int page, int itemsPerPage = 8)
        {
            var reviews = await this.reviewRepository
                .All()
                .OrderByDescending(x => x.CreatedOn)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .To<ReviewInListViewModel>()
                .ToListAsync();

            foreach (var review in reviews)
            {
                ModelInput sampleData = new ModelInput()
                {
                    Review = review.ReviewText,
                };

                var predictionResult = ReviewPredictionModel.Predict(sampleData);

                review.Prediction = predictionResult.Prediction.ToUpper();
            }

            return reviews;
        }

        public async Task<List<ReviewViewModel>> AllForClubAsync(int clubId)
        {
            return await this.reviewRepository.All().Where(x => x.ClubId == clubId).To<ReviewViewModel>().ToListAsync();
        }

        public async Task<ReviewViewModel> CreateAsync(string userId, int atheleteId, CreateReviewInputModel model)
        {
            var review = await this.reviewRepository.All()
                .Where(x => x.ClubId == model.ClubId && x.OwnerId == atheleteId)
                .FirstOrDefaultAsync();

            var club = await this.clubRepository.All()
                .Where(x => x.Id == model.ClubId)
                .FirstOrDefaultAsync();

            var user = await this.applicationUserRepository.All()
                .Where(x => x.Id == userId)
                .Include(x => x.Image)
                .FirstOrDefaultAsync();

            if (review != null || club == null || user == null)
            {
                throw new ArgumentException();
            }

            review = new Review()
            {
                OwnerId = atheleteId,
                ClubId = model.ClubId,
                ReviewText = model.ReviewText,
                Rating = model.Rating,
            };

            await this.reviewRepository.AddAsync(review);
            await this.reviewRepository.SaveChangesAsync();

            return new ReviewViewModel()
            {
                Id = review.Id,
                Rating = review.Rating,
                ReviewText = review.ReviewText,
                UserProfilePic = user.Image.URL,
                UserName = user.UserName,
            };
        }

        public async Task DeleteAsync(int clubId)
        {
            var review = await this.reviewRepository.AllAsNoTracking()
                .Where(s => s.Id == clubId)
                .FirstOrDefaultAsync();

            if (review == null)
            {
                throw new ArgumentException();
            }

            this.reviewRepository.Delete(review);
            await this.reviewRepository.SaveChangesAsync();
        }

        public async Task<List<ReviewInProfileViewModel>> GetAllForAthleteAsync(int athleteId)
        {
            return await this.reviewRepository.All()
                .Where(x => x.OwnerId == athleteId)
                .To<ReviewInProfileViewModel>()
                .ToListAsync();
        }

        public double GetAverageForClub(int clubId)
        {
            if (this.reviewRepository.AllAsNoTracking().Where(x => x.ClubId == clubId).Count() > 0)
            {

                var avarateRatings = this.reviewRepository.AllAsNoTracking()
                    .Where(x => x.ClubId == clubId)
                    .Average(x => x.Rating);

                return avarateRatings;
            }

            return 0.0;
        }

        public int GetCount()
        {
            return this.reviewRepository.AllAsNoTracking().Count();
        }
    }
}
