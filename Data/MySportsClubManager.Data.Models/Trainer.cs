namespace MySportsClubManager.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using MySportsClubManager.Data.Common.Models;

    public class Trainer : BaseDeletableModel<int>
    {
        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey(nameof(OwnedClub))]
        public int? OwnedClubId { get; set; }

        public Club OwnedClub { get; set; }
    }
}
