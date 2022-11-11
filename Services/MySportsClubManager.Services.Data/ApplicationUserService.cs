namespace MySportsClubManager.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.ViewModels.ApplicationUser;

    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> applicationUserRepository;

        public ApplicationUserService(IDeletableEntityRepository<ApplicationUser> applicationUserRepository)
        {
            this.applicationUserRepository = applicationUserRepository;
        }

        public async Task<List<ApplicationUserInformationViewModel>> AllAsync()
        {
            return await this.applicationUserRepository.All()
                .Select(u => new ApplicationUserInformationViewModel()
                {
                    Id = u.Id.ToString(),
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    UserName = u.UserName,
                    ImageUrl = u.ImageUrl,
                })
                .ToListAsync();
        }
    }
}
