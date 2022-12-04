namespace MySportsClubManager.Web.ViewModels.Review
{
    using AutoMapper;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;

    public class ReviewInProfileViewModel : IMapFrom<Review>, IHaveCustomMappings
    {
        public int ClubId { get; set; }

        public string ClubImage { get; set; }

        public string ClubName { get; set; }

        public string ReviewText { get; set; }

        public int Rating { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Review, ReviewInProfileViewModel>()
                .ForMember(x => x.ClubImage, opt =>
                opt.MapFrom(r => r.Club.Image.URL))
                .ForMember(x => x.ClubName, opt =>
                opt.MapFrom(r => r.Club.Name));
        }
    }
}
