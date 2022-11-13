namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MySportsClubManager.Services.Data.Contracts.Base;
    using MySportsClubManager.Web.ViewModels.Sport;

    public interface ISportService: IPaginationBase
    {
        Task<List<SportInDropdownViewModel>> AllForInputAsync();

        Task Create(CreateSportInputModel model);
    }
}
