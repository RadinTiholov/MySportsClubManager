namespace MySportsClubManager.Web.ViewModels.ApplicationUser
{
    using System.Collections.Generic;

    using AutoMapper;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.Club;
    using MySportsClubManager.Web.ViewModels.Review;
    using MySportsClubManager.Web.ViewModels.Win;

    public class ProfileViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ImageUrl { get; set; }

        public ClubInListViewModel Club { get; set; }

        public List<ReviewInProfileViewModel> Reviews { get; set; }

        public List<AchievementInListViewModel> Achievements { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, ProfileViewModel>()
                .ForMember(x => x.ImageUrl, opt =>
                opt.MapFrom(u => u.Image.URL));
        }
    }
}
