namespace MySportsClubManager.Web.ViewModels.Training
{
    using System.Collections.Generic;

    using MySportsClubManager.Web.ViewModels.Club;

    public class TrainingListViewModel
    {
        public IEnumerable<TrainingInListViewModel> Trainings { get; set; }

        public ClubDetailsViewModel Club { get; set; }
    }
}
