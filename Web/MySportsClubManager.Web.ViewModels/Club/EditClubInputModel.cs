namespace MySportsClubManager.Web.ViewModels.Club
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;
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

        [DataType(DataType.Upload)]
        public IList<IFormFile> ImageFiles { get; set; }

        public string[] ImageUrls { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Club, EditClubInputModel>()
                .ForMember(x => x.ImageUrls, opt =>
                opt.MapFrom(c => c.Images.Select(i => i.URL).ToArray()));
        }
    }
}
