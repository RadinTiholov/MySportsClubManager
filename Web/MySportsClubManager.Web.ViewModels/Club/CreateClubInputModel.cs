namespace MySportsClubManager.Web.ViewModels.Club
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
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
        [Range(typeof(decimal), MinFee, MaxFee, ConvertValueInInvariantCulture = true)]
        public decimal Fee { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        public IList<IFormFile> ImageFiles { get; set; }
    }
}
