namespace MySportsClubManager.Web.ViewModels.Review
{
    using System.Collections.Generic;

    using MySportsClubManager.Web.ViewModels.Base;

    public class ReviewListViewModel : PaginationViewModel
    {
        public IEnumerable<ReviewInListViewModel> Reviews { get; set; }
    }
}
