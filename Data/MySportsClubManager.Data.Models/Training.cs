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

    using static MySportsClubManager.Data.Common.DataValidation.Training;

    public class Training : BaseDeletableModel<int>
    {
        public Training()
        {
            this.EnrolledAthletes = new HashSet<Athlete>();
        }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        [ForeignKey(nameof(Club))]
        public int ClubId { get; set; }

        public Club Club { get; set; }

        [Required]
        [MaxLength(TopicMaxLength)]
        public string Topic { get; set; }

        public virtual ICollection<Athlete> EnrolledAthletes { get; set; }
    }
}
