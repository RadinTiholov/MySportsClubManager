namespace MySportsClubManager.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MySportsClubManager.Services.Data.Contracts.Base;
    using MySportsClubManager.Web.ViewModels.Club;

    public interface IClubService : IPaginationBase
    {
        Task Create(CreateClubInputModel model, string ownerId);
    }
}
