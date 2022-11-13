namespace MySportsClubManager.Web.ViewModels.Club
{
    using System.Collections.Generic;

    using MySportsClubManager.Web.ViewModels.Base;
    using MySportsClubManager.Web.ViewModels.Sport;

    public class ClubListViewModel : PaginationViewModel
    {
        public IEnumerable<ClubInListViewModel> Clubs { get; set; }
    }
}
