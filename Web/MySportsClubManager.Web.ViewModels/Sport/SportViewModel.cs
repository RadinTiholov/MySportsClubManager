namespace MySportsClubManager.Web.ViewModels.Sport
{
    using System;

    using MySportsClubManager.Web.ViewModels.Country;
    using MySportsClubManager.Web.ViewModels.Creator;

    public class SportViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreationDate { get; set; }

        public string ImageUrl { get; set; }

        public CountryViewModel Country { get; set; }

        public CreatorViewModel Creator { get; set; }
    }
}
