namespace MySportsClubManager.Web.ViewModels.Club
{
    using System.Linq;

    using AutoMapper;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.Base;
    using MySportsClubManager.Web.ViewModels.Country;
    using MySportsClubManager.Web.ViewModels.Creator;
    using MySportsClubManager.Web.ViewModels.Sport;
    using MySportsClubManager.Web.ViewModels.Trainer;

    public class ClubDetailsViewModel : IMapFrom<Club>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string OwnerId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public decimal Fee { get; set; }

        public int SportId { get; set; }

        public string Sport { get; set; }

        public TrainerInDetailsViewModel Trainer { get; set; }

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Club, ClubDetailsViewModel>()
                .ForMember(x => x.Sport, opt =>
                opt.MapFrom(c => c.Sport.Name.ToString()))
                .ForMember(x => x.Trainer, opt =>
                opt.MapFrom(c => new TrainerInDetailsViewModel()
                {
                    FirstName = c.Trainer.ApplicationUser.FirstName,
                    LastName = c.Trainer.ApplicationUser.LastName,
                    Email = c.Trainer.ApplicationUser.Email,
                    ProfilePic = c.Trainer.ApplicationUser.Image.URL,
                    Id = c.Trainer.Id,
                }))
                .ForMember(x => x.ImageUrl, opt =>
                opt.MapFrom(s => s.Image.URL.ToString()))
                .ForMember(x => x.OwnerId, opt =>
                opt.MapFrom(c => c.Trainer.ApplicationUserId));
        }
    }
}
