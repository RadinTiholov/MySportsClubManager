namespace MySportsClubManager.Web.ViewModels.Contest
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using MySportsClubManager.Common;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.Athlete;

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

        public List<AthleteInListViewModel> Athletes { get; set; }

        public List<WinnerInListViewModel> Champions { get; set; }

        public int ClubsEnrolledCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Contest, ContestDetailsViewModel>()
                .ForMember(x => x.Sport, opt =>
                opt.MapFrom(c => c.Sport.Name.ToString()))
                .ForMember(x => x.ImageUrl, opt =>
                opt.MapFrom(c => c.Image.URL))
                .ForMember(x => x.Date, opt =>
                opt.MapFrom(c => c.Date.ToString(GlobalConstants.DateFormat)))
                .ForMember(x => x.ClubsEnrolledCount, opt =>
                opt.MapFrom(c => c.Clubs.Count()))
                .ForMember(x => x.Athletes, opt =>
                opt.MapFrom(c => c.Participants.Select(x => new AthleteInListViewModel() { Name = x.ApplicationUser.UserName, ProfilePic = x.ApplicationUser.Image.URL })))
                .ForMember(x => x.Champions, opt =>
                opt.MapFrom(c => c.Participants.Where(p => c.Wins.Any(w => w.AthleteId == p.Id)).Select(x => new WinnerInListViewModel() { Name = x.ApplicationUser.UserName, ProfilePic = x.ApplicationUser.Image.URL, Place = x.Wins.Where(w => w.AthleteId == x.Id).FirstOrDefault().Place })));
        }
    }
}
