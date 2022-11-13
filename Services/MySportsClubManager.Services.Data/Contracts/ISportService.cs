namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MySportsClubManager.Web.ViewModels.Sport;

    public interface ISportService
    {
        Task<List<SportInDropdownViewModel>> AllForInputAsync();

        Task Create(CreateSportInputModel model);

        Task<List<T>> AllAsync<T>(int page, int itemsPerPage = 8);

        int GetCount();
    }
}
