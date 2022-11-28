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

        public string Year { get; set; }

        public string Duration { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Training, TrainingInListViewModel>()
                .ForMember(x => x.Day, opt =>
                opt.MapFrom(s => s.Date.Day))
                .ForMember(x => x.Month, opt =>
                opt.MapFrom(s => s.Date.ToString("MMMM").Substring(0, 3).ToUpper()))
                .ForMember(x => x.Year, opt =>
                opt.MapFrom(s => s.Date.Year))
                .ForMember(x => x.Duration, opt =>
                opt.MapFrom(s => s.Duration.ToString(GlobalConstants.TimeFormat)));
        }
    }
}
