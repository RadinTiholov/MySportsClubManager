namespace MySportsClubManager.Web.ViewModels.Club
{
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;

    public class ClubsInDropdownViewModel : IMapFrom<Club>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
