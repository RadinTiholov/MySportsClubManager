namespace MySportsClubManager.Web.ViewModels.Base
{
    using System;

    public class PaginationViewModel
    {
        public int PageNumber { get; set; }

        public int SportsCount { get; set; }

        public int ItemsPerPage { get; set; }

        public int PreviousPageNumber => this.PageNumber - 1;

        public int NextPageNumber => this.PageNumber + 1;

        public bool HasPreviousPage => this.PageNumber > 1;

        public bool HasNextPage => this.PageNumber < this.PagesCount;

        public int PagesCount => (int)Math.Ceiling((double)this.SportsCount / this.ItemsPerPage);
    }
}
