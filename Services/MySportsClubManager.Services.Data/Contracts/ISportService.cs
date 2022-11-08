namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MySportsClubManager.Web.ViewModels.Sport;

    public interface ISportService
    {
        Task<List<SportListViewModel>> AllForInputAsync();

        Task Create(CreateSportInputModel model);
    }
}
