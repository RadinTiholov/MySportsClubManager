namespace MySportsClubManager.Web.ViewModels.Contest
{
    using System.Collections.Generic;

    using MySportsClubManager.Web.ViewModels.Athlete;

    public class ChooseWinnerInputModel
    {
        public int ContestId { get; set; }

        public int FirsPlaceId { get; set; }

        public int SecondPlaceId { get; set; }

        public int ThirdPlaceId { get; set; }

        public List<AthleteInDropdownViewModel> Athletes { get; set; }
    }
}
