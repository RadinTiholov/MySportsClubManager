namespace MySportsClubManager.Data.Models
{
    using MySportsClubManager.Data.Common.Models;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class Athlete : BaseDeletableModel<int>
    {
        public Athlete()
        {
            this.Reviews = new HashSet<Review>();
            this.Trainings = new HashSet<Training>();
            this.Contests = new HashSet<Contest>();
            this.Wins = new HashSet<Win>();
        }

        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey(nameof(EnrolledClub))]
        public int? EnrolledClubId { get; set; }

        public Club EnrolledClub { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<Training> Trainings { get; set; }

        public virtual ICollection<Contest> Contests { get; set; }

        public virtual ICollection<Win> Wins { get; set; }
    }
}
