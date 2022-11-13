namespace MySportsClubManager.Web.ViewModels.Club
{
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;

    public class ClubInListViewModel : IMapFrom<Club>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }
    }
}
