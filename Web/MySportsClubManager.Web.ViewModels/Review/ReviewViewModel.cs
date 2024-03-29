﻿namespace MySportsClubManager.Web.ViewModels.Review
{
    using AutoMapper;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;

    public class ReviewViewModel : IMapFrom<Review>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string ReviewText { get; set; }

        public int Rating { get; set; }

        public string UserProfilePic { get; set; }

        public string UserName { get; set; }

        public string UserId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Review, ReviewViewModel>()
                .ForMember(x => x.UserProfilePic, opt =>
                opt.MapFrom(r => r.Owner.ApplicationUser.Image.URL))
                .ForMember(x => x.UserName, opt =>
                opt.MapFrom(r => r.Owner.ApplicationUser.UserName))
                .ForMember(x => x.UserId, opt =>
                opt.MapFrom(r => r.Owner.ApplicationUserId));
        }
    }
}
