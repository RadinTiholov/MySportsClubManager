namespace MySportsClubManager.Web.ViewModels.Trainer
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using AutoMapper;
    using MySportsClubManager.Services.Mapping;

    using static MySportsClubManager.Data.Common.DataValidation.Trainer;

    using Trainer = MySportsClubManager.Data.Models.Trainer;

    public class ContactTrainerInputModel : IMapFrom<Trainer>, IHaveCustomMappings
    {
        public string TrainerEmail { get; set; }

        public string ImageUrl { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [NotMapped]
        [EmailAddress]
        public string SenderEmail { get; set; }

        [Required]
        [NotMapped]
        [StringLength(TopicMaxLength, MinimumLength = TopicMinLength)]
        public string Topic { get; set; }

        [Required]
        [NotMapped]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength)]
        public string EmailText { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Trainer, ContactTrainerInputModel>()
                .ForMember(x => x.ImageUrl, opt =>
                opt.MapFrom(s => s.ApplicationUser.Image.URL))
                .ForMember(x => x.FirstName, opt =>
                opt.MapFrom(s => s.ApplicationUser.FirstName))
                .ForMember(x => x.LastName, opt =>
                opt.MapFrom(s => s.ApplicationUser.LastName))
                .ForMember(x => x.TrainerEmail, opt =>
                opt.MapFrom(s => s.ApplicationUser.Email));
        }
    }
}
