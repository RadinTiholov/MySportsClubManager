namespace MySportsClubManager.Web.ViewModels.Club
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MySportsClubManager.Web.ViewModels.Sport;

    using static MySportsClubManager.Data.Common.DataValidation.Club;

    public class CreateClubInputModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; }

        [Required]
        public int SportId { get; set; }

        public IEnumerable<SportInDropdownViewModel> Sports { get; set; } = new List<SportInDropdownViewModel>();

        [Required]
        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength)]
        public string Address { get; set; }

        [Required]
        [Range(typeof(decimal), "0.0", "9999", ConvertValueInInvariantCulture = true)]
        public decimal Fee { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }
    }
}
