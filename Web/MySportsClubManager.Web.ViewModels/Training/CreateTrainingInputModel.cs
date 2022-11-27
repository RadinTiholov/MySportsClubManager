namespace MySportsClubManager.Web.ViewModels.Training
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MySportsClubManager.Web.ViewModels.Club;

    using static MySportsClubManager.Data.Common.DataValidation.Training;

    public class CreateTrainingInputModel
    {
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

        public IEnumerable<ClubsInDropdownViewModel> Clubs { get; set; } = new List<ClubsInDropdownViewModel>();
    }
}
