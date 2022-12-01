namespace MySportsClubManager.Web.ViewModels.Review
{
    using MySportsClubManager.Web.ViewModels.ApplicationUser;

    public class ReviewViewModel
    {
        public int Id { get; set; }

        public string ReviewText { get; set; }

        public int Rating { get; set; }

        public string UserProfilePic { get; set; }

        public string UserName { get; set; }
    }
}
