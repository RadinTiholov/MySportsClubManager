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
    using static MySportsClubManager.Data.Common.DataValidation.Sport;

    public class Sport : BaseModel<int>
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public string ImageUrl { get; set; }

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
