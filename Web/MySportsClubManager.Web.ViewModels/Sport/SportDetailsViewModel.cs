namespace MySportsClubManager.Web.ViewModels.Sport
{
    using AutoMapper;
    using MySportsClubManager.Common;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.Country;
    using MySportsClubManager.Web.ViewModels.Creator;
    using System.Collections.Generic;

    public class SportDetailsViewModel : IMapFrom<Sport>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CreationDate { get; set; }

        public string ImageUrl { get; set; }

        public CountryViewModel Country { get; set; }

        public CreatorViewModel Creator { get; set; }

        public List<SportInListViewModel> RecentSports { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Sport, SportDetailsViewModel>()
                .ForMember(x => x.Country, opt =>
                opt.MapFrom(s => new CountryViewModel() { Id = s.CountryId, Name = s.Country.Name }))
                .ForMember(x => x.Creator, opt =>
                opt.MapFrom(s => new CreatorViewModel() { Id = s.CountryId, Name = s.Creator.Name }))
                .ForMember(x => x.ImageUrl, opt =>
                opt.MapFrom(s => s.Image.URL.ToString()))
                .ForMember(x => x.CreationDate, opt =>
                opt.MapFrom(s => s.CreationDate.ToString(GlobalConstants.DateFormat)));
        }
    }
}
