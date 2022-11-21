namespace MySportsClubManager.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using MySportsClubManager.Data.Common.Models;

    using static MySportsClubManager.Data.Common.DataValidation.Contest;

    public class Contest : BaseDeletableModel<int>
    {
        public Contest()
        {
            this.Clubs = new HashSet<Club>();
            this.Participants = new HashSet<Athlete>();
            this.Wins = new HashSet<Win>();
        }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [ForeignKey(nameof(Image))]
        public int ImageId { get; set; }

        public Image Image { get; set; }

        [ForeignKey(nameof(Sport))]
        [Required]
        public int SportId { get; set; }

        [Required]
        public Sport Sport { get; set; }

        public virtual ICollection<Club> Clubs { get; set; }

        public virtual ICollection<Athlete> Participants { get; set; }

        public virtual ICollection<Win> Wins { get; set; }
    }
}
