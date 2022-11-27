namespace MySportsClubManager.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Reflection.Metadata.Ecma335;
    using System.Text;
    using System.Threading.Tasks;

    using MySportsClubManager.Data.Common.Models;

    using static MySportsClubManager.Data.Common.DataValidation.Club;

    public class Club : BaseDeletableModel<int>
    {
        public Club()
        {
            this.Athletes = new HashSet<Athlete>();
            this.Reviews = new HashSet<Review>();
            this.Trainings = new HashSet<Training>();
            this.Contests = new HashSet<Contest>();
        }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [ForeignKey(nameof(Sport))]
        [Required]
        public int SportId { get; set; }

        [Required]
        public Sport Sport { get; set; }

        [Required]
        [ForeignKey(nameof(Trainer))]
        public int TrainerId { get; set; }

        [Required]
        public Trainer Trainer { get; set; }

        [Required]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; }

        [Required]
        public decimal Fee { get; set; }

        [Required]
        [ForeignKey(nameof(Image))]
        public int ImageId { get; set; }

        public Image Image { get; set; }

        public ICollection<Athlete> Athletes { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<Training> Trainings { get; set; }

        public virtual ICollection<Contest> Contests { get; set; }
    }
}
