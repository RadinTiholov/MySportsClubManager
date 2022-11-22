namespace MySportsClubManager.Web.ViewModels.Contest
{
    using System.Collections.Generic;

    using MySportsClubManager.Web.ViewModels.Base;
    using MySportsClubManager.Web.ViewModels.Sport;

    public class ContestListViewModel : PaginationViewModel
    {
        public IEnumerable<ContestInListViewModel> Contest { get; set; }

        public IEnumerable<SportInDropdownViewModel> RecentSports { get; set; }
    }
}
