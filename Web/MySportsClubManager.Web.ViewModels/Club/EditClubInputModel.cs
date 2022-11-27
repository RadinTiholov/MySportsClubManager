namespace MySportsClubManager.Web.ViewModels.Club
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.Infrastructure.Attributes;
    using MySportsClubManager.Web.ViewModels.Sport;

    using static MySportsClubManager.Data.Common.DataValidation.Club;

    public class EditClubInputModel : IMapFrom<Club>, IHaveCustomMappings
    {
        public int Id { get; set; }

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

        [AllowedFileExtensions]
        [DataType(DataType.Upload)]
        public IFormFile ImageFile { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Club, EditClubInputModel>()
                .ForMember(x => x.ImageUrl, opt =>
                opt.MapFrom(s => s.Image.URL));
        }
    }
}
