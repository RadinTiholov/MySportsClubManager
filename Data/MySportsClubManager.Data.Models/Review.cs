﻿namespace MySportsClubManager.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using MySportsClubManager.Data.Common.Models;

    using static MySportsClubManager.Data.Common.DataValidation.Review;

    public class Review : BaseDeletableModel<int>
    {
        [Required]
        [ForeignKey(nameof(Owner))]
        public int OwnerId { get; set; }

        public Athlete Owner { get; set; }

        [Required]
        [ForeignKey(nameof(Club))]
        public int ClubId { get; set; }

        public Club Club { get; set; }

        [Required]
        [MaxLength(ReviewTextMaxLength)]
        public string ReviewText { get; set; }

        [Required]
        public int Rating { get; set; }
    }
}
