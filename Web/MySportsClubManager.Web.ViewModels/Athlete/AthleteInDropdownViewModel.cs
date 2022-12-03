namespace MySportsClubManager.Web.ViewModels.Athlete
{
    using AutoMapper;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;

    public class AthleteInDropdownViewModel : IMapFrom<Athlete>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Athlete, AthleteInDropdownViewModel>()
                .ForMember(x => x.Name, opt =>
                opt.MapFrom(a => a.ApplicationUser.UserName));
        }
    }
}
