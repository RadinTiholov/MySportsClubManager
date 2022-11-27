namespace MySportsClubManager.Web.ViewModels.Club
{
    using System.Linq;

    using AutoMapper;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.Base;

    public class ClubInListViewModel : IMapFrom<Club>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ImageViewModel Image { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Club, ClubInListViewModel>()
                .ForMember(x => x.Image, opt =>
                opt.MapFrom(i => new ImageViewModel() { URL = i.Image.URL }));
        }
    }
}
