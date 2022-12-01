namespace MySportsClubManager.Web.ViewModels.Review
{
    using System.ComponentModel.DataAnnotations;

    using static MySportsClubManager.Data.Common.DataValidation.Review;

    public class CreateReviewInputModel
    {
        [Required]
        public int ClubId { get; set; }

        [Required]
        [StringLength(ReviewTextMaxLength, MinimumLength = ReviewTextMinLength)]
        public string ReviewText { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
    }
}
