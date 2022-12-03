namespace MySportsClubManager.Web.ViewModels.Athlete
{
    using AutoMapper;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;

    public class AthleteInListViewModel : IMapFrom<Athlete>, IHaveCustomMappings
    {
        public string Name { get; set; } = null!;

        public string ProfilePic { get; set; } = null!;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Athlete, AthleteInListViewModel>()
                .ForMember(x => x.Name, opt =>
                opt.MapFrom(a => a.ApplicationUser.UserName))
                .ForMember(x => x.ProfilePic, opt =>
                opt.MapFrom(a => a.ApplicationUser.Image.URL));
        }
    }
}
