namespace MySportsClubManager.Web.ViewModels.Sport
{
    using System;
    using System.Collections.Generic;

    using MySportsClubManager.Web.ViewModels.Base;

    public class SportListViewModel : PaginationViewModel
    {
        public IEnumerable<SportInListViewModel> Sports { get; set; }
    }
}
