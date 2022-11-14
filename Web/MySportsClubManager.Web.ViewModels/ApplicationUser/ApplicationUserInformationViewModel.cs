namespace MySportsClubManager.Web.ViewModels.ApplicationUser
{
    using AutoMapper;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.Base;
    using MySportsClubManager.Web.ViewModels.Sport;

    public class ApplicationUserInformationViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public ImageViewModel Image { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, ApplicationUserInformationViewModel>()
                .ForMember(x => x.Image, opt =>
                opt.MapFrom(i => new ImageViewModel() { URL = i.Image.URL }));
        }
    }
}
