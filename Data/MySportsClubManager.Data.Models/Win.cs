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

    public class Win : BaseModel<int>
    {
        [ForeignKey(nameof(Athlete))]
        [Required]
        public int AthleteId { get; set; }

        public Athlete Athlete { get; set; }

        [ForeignKey(nameof(Contest))]
        [Required]
        public int ContestId { get; set; }

        public Contest Contest { get; set; }

        [Required]
        public int Place { get; set; }
    }
}
