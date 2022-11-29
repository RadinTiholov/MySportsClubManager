namespace MySportsClubManager.Web.ViewModels.Training
{
    using AutoMapper;
    using MySportsClubManager.Common;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;

    public class TrainingInListViewModel : IMapFrom<Training>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Topic { get; set; }

        public string Day { get; set; }

        public string Month { get; set; }

        public int MonthWithDigits { get; set; }

        public string Year { get; set; }

        public string Duration { get; set; }

        public int ParticipantsCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Training, TrainingInListViewModel>()
                .ForMember(x => x.Day, opt =>
                opt.MapFrom(t => t.Date.Day))
                .ForMember(x => x.Month, opt =>
                opt.MapFrom(t => t.Date.ToString("MMMM").Substring(0, 3).ToUpper()))
                .ForMember(x => x.Year, opt =>
                opt.MapFrom(t => t.Date.Year))
                .ForMember(x => x.Duration, opt =>
                opt.MapFrom(t => t.Duration.ToString(GlobalConstants.TimeFormat)))
                .ForMember(x => x.ParticipantsCount, opt =>
                opt.MapFrom(t => t.EnrolledAthletes.Count))
                .ForMember(x => x.MonthWithDigits, opt =>
                opt.MapFrom(t => t.Date.Month));
        }
    }
}
