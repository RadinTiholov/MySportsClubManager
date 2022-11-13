namespace MySportsClubManager.Services.Data.Contracts.Base
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPaginationBase
    {
        Task<List<T>> AllAsync<T>(int page, int itemsPerPage = 8);

        int GetCount();
    }
}
