namespace MySportsClubManager.Web.ViewModels.Contest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.Infrastructure.Attributes;
    using MySportsClubManager.Web.ViewModels.Sport;

    using static MySportsClubManager.Data.Common.DataValidation.Contest;

    public class EditContestViewModel : IMapFrom<Contest>, IHaveCustomMappings
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; }

        [Required]
        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength)]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public int SportId { get; set; }

        public IEnumerable<SportInDropdownViewModel> Sports { get; set; } = new List<SportInDropdownViewModel>();

        [AllowedFileExtensions]
        [DataType(DataType.Upload)]
        public IFormFile ImageFile { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Contest, EditContestViewModel>()
               .ForMember(x => x.ImageUrl, opt =>
               opt.MapFrom(s => s.Image.URL));
        }
    }
}
