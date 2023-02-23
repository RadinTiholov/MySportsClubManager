namespace MySportsClubManager.Web.ViewModels.Message
{
    using AutoMapper;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;

    public class MessageInListViewModel : IMapFrom<Message>, IHaveCustomMappings
    {
        public string Text { get; set; } = null!;

        public string SenderUsername { get; set; } = null!;

        public string SenderImage { get; set; } = null!;

        public string ReceiverUsername { get; set; } = null!;

        public string ReceiverImage { get; set; } = null!;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Message, MessageInListViewModel>()
                .ForMember(x => x.SenderUsername, opt =>
                    opt.MapFrom(s => s.Sender.ApplicationUser.UserName))
                .ForMember(x => x.SenderImage, opt =>
                    opt.MapFrom(s => s.Sender.ApplicationUser.Image.URL))
                .ForMember(x => x.ReceiverUsername, opt =>
                    opt.MapFrom(r => r.Receiver.ApplicationUser.UserName))
                .ForMember(x => x.ReceiverImage, opt =>
                    opt.MapFrom(r => r.Receiver.ApplicationUser.Image.URL));
        }
    }
}
