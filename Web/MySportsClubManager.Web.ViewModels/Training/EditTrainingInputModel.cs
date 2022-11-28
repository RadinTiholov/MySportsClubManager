namespace MySportsClubManager.Web.ViewModels.Training
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.Club;

    using static MySportsClubManager.Data.Common.DataValidation.Training;

    public class EditTrainingInputModel : IMapFrom<Training>, IHaveCustomMappings
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(TopicMaxLength, MinimumLength = TopicMinLength)]
        public string Topic { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan Duration { get; set; }

        [Required]
        public int ClubId { get; set; }

        public int TrainerId { get; set; }

        public IEnumerable<ClubsInDropdownViewModel> Clubs { get; set; } = new List<ClubsInDropdownViewModel>();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Training, EditTrainingInputModel>()
                .ForMember(x => x.TrainerId, opt =>
                opt.MapFrom(t => t.Club.TrainerId));
        }
    }
}
