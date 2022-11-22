namespace MySportsClubManager.Web.ViewModels.Contest
{
    using AutoMapper;
    using MySportsClubManager.Common;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;

    public class ContestDetailsViewModel : IMapFrom<Contest>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string Date { get; set; }

        public string ImageUrl { get; set; }

        public string SportId { get; set; }

        public string Sport { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Contest, ContestDetailsViewModel>()
                .ForMember(x => x.Sport, opt =>
                opt.MapFrom(c => c.Sport.Name.ToString()))
                .ForMember(x => x.ImageUrl, opt =>
                opt.MapFrom(c => c.Image.URL))
                .ForMember(x => x.Date, opt =>
                opt.MapFrom(s => s.Date.ToString(GlobalConstants.DateFormat)));
        }
    }
}
