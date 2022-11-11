namespace MySportsClubManager.Web.ViewModels.Club
{
    using MySportsClubManager.Web.ViewModels.Sport;

    public class ClubViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Sport { get; set; }

        public string Address { get; set; }

        public decimal Fee { get; set; }

        public string ImageUrl { get; set; }

        public string OwnerName { get; set; }
    }
}
