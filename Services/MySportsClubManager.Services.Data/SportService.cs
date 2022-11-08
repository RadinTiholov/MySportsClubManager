namespace MySportsClubManager.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.ViewModels.Sport;

    public class SportService : ISportService
    {
        private readonly IRepository<Sport> sportsRepository;

        public SportService(IRepository<Sport> sportsRepository)
        {
            this.sportsRepository = sportsRepository;
        }

        public async Task<List<SportListViewModel>> AllForInputAsync()
        {
            return await this.sportsRepository
                .All()
                .Select(x => new SportListViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToListAsync();
        }
    }
}
