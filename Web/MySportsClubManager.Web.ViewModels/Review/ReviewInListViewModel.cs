namespace MySportsClubManager.Web.ViewModels.Review
{
    using AutoMapper;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;

    public class ReviewInListViewModel : IMapFrom<Review>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string ReviewText { get; set; }

        public int Rating { get; set; }

        public string ClubName { get; set; }

        public int ClubId { get; set; }

        public string Prediction { get; set; }

        public string OwnerName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Review, ReviewInListViewModel>()
                .ForMember(x => x.ClubName, opt =>
                opt.MapFrom(r => r.Club.Name))
                .ForMember(x => x.OwnerName, opt =>
                opt.MapFrom(r => r.Owner.ApplicationUser.UserName));
        }
    }
}
