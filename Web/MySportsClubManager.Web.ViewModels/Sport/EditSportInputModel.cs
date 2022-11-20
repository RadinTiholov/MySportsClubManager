namespace MySportsClubManager.Web.ViewModels.Sport
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using MySportsClubManager.Common;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.Infrastructure.Attributes;

    using static MySportsClubManager.Data.Common.DataValidation.Country;
    using static MySportsClubManager.Data.Common.DataValidation.Creator;
    using static MySportsClubManager.Data.Common.DataValidation.Sport;

    public class EditSportInputModel : IMapFrom<Sport>, IHaveCustomMappings
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(SportNameMaxLength, MinimumLength = SportNameMinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(SportDescriptionMaxLength, MinimumLength = SportDescriptionMinLength)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        [AllowedFileExtensions]
        [DataType(DataType.Upload)]
        public IFormFile ImageFile { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [StringLength(CountryNameMaxLength, MinimumLength = CountryNameMinLength)]
        public string Country { get; set; }

        [Required]
        [StringLength(CreatorNameMaxLength, MinimumLength = CreatorNameMinLength)]
        public string Creator { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Sport, EditSportInputModel>()
                .ForMember(x => x.Country, opt =>
                opt.MapFrom(s => s.Country.Name))
                .ForMember(x => x.Creator, opt =>
                opt.MapFrom(s => s.Creator.Name))
                .ForMember(x => x.ImageUrl, opt =>
                opt.MapFrom(s => s.Image.URL));
        }
    }
}
