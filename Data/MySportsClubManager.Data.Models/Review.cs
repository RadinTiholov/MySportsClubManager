namespace MySportsClubManager.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using MySportsClubManager.Data.Common.Models;

    using static MySportsClubManager.Data.Common.DataValidation.Review;

    public class Review : BaseDeletableModel<int>
    {
        [Required]
        [ForeignKey(nameof(Owner))]
        public int OwnerId { get; set; }

        public ApplicationUser Owner { get; set; }

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
