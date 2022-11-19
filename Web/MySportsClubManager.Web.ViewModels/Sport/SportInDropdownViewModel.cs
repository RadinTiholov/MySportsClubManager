namespace MySportsClubManager.Web.ViewModels.Sport
{
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Mapping;

    public class SportInDropdownViewModel : IMapFrom<Sport>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
