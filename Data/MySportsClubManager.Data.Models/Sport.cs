namespace MySportsClubManager.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using MySportsClubManager.Data.Common.Models;

    using static MySportsClubManager.Data.Common.DataValidation.Sport;

    public class Sport : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(SportNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(SportDescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        [ForeignKey(nameof(Image))]
        public int ImageId { get; set; }

        public Image Image { get; set; }

        [Required]
        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }

        [Required]
        public Country Country { get; set; }

        [Required]
        [ForeignKey(nameof(Creator))]
        public int CreatorId { get; set; }

        [Required]
        public Creator Creator { get; set; }
    }
}
