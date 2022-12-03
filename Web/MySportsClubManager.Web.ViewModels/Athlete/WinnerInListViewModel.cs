namespace MySportsClubManager.Web.ViewModels.Athlete
{
    using System.Linq;

    using AutoMapper;
    using MySportsClubManager.Data.Models;

    public class WinnerInListViewModel
    {
        public int Place { get; set; }

        public string Name { get; set; } = null!;

        public string ProfilePic { get; set; } = null!;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Athlete, WinnerInListViewModel>()
                .ForMember(x => x.Name, opt =>
                opt.MapFrom(a => a.ApplicationUser.UserName))
                .ForMember(x => x.ProfilePic, opt =>
                opt.MapFrom(a => a.ApplicationUser.Image.URL))
                .ForMember(x => x.Place, opt =>
                opt.MapFrom(a => a.Wins.Where(x => x.AthleteId == a.Id).FirstOrDefault().Place));
        }
    }
}