namespace MySportsClubManager.Web.ViewModels.Sport
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static MySportsClubManager.Data.Common.DataValidation.Country;
    using static MySportsClubManager.Data.Common.DataValidation.Creator;
    using static MySportsClubManager.Data.Common.DataValidation.Sport;

    public class CreateSportInputModel
    {
        [Required]
        [StringLength(SportNameMaxLength, MinimumLength=SportDescriptionMinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(SportDescriptionMaxLength, MinimumLength=SportDescriptionMinLength)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        [Required]
        [StringLength(CountryNameMaxLength, MinimumLength=CountryNameMinLength)]
        public string Country { get; set; }

        [Required]
        [StringLength(CreatorNameMaxLength, MinimumLength = CreatorNameMinLength)]
        public string Creator { get; set; }
    }
}
