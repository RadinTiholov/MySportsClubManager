namespace MySportsClubManager.Web.ViewModels.Sport
{
    using System;

    using AutoMapper;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.Country;
    using MySportsClubManager.Web.ViewModels.Creator;

    public class SportInListViewModel : IMapFrom<Sport>/*, IHaveCustomMappings*/
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        //public CountryViewModel Country { get; set; }

        //public CreatorViewModel Creator { get; set; }

        //public void CreateMappings(IProfileExpression configuration)
        //{
        //    configuration.CreateMap<Sport, SportInListViewModel>()
        //        .ForMember(x => x.Country, opt =>
        //        opt.MapFrom(s => new CountryViewModel() { Id = s.CountryId, Name = s.Country.Name }))
        //        .ForMember(x => x.Creator, opt =>
        //        opt.MapFrom(s => new CreatorViewModel() { Id = s.CountryId, Name = s.Country.Name }));
        //}
    }
}
