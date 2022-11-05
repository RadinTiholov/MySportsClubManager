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

    public class Training : BaseDeletableModel<int>
    {
        public Training()
        {
            this.EnrolledUsers = new HashSet<ApplicationUser>();
        }

        public DateTime Date { get; set; }

        public TimeSpan Duration { get; set; }

        [Required]
        [ForeignKey(nameof(Club))]
        public int ClubId { get; set; }

        public Club Club { get; set; }

        public string Topic { get; set; }

        public virtual ICollection<ApplicationUser> EnrolledUsers { get; set; }
    }
}
