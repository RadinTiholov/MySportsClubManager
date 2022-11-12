namespace MySportsClubManager.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using MySportsClubManager.Common;
    using MySportsClubManager.Data;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Web.ViewModels.ApplicationUser;

    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> applicationUserRepository;
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public ApplicationUserService(IDeletableEntityRepository<ApplicationUser> applicationUserRepository, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.applicationUserRepository = applicationUserRepository;
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<List<ApplicationUserInformationViewModel>> AllAsync()
        {
            return await this.applicationUserRepository.All()
                .Include(x => x.Roles)
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

        public async Task AssignUserToRole(string id, string roleName)
        {
            var user = await this.applicationUserRepository.All()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new InvalidOperationException(ServiceErrorMessages.UserNotFoundMessage);
            }

            if (await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName) || await this.userManager.IsInRoleAsync(user, GlobalConstants.TrainerRoleName))
            {
                throw new InvalidOperationException(ServiceErrorMessages.UserAlreadyInRoleMessage);
            }

            await this.db.UserRoles.AddAsync(new IdentityUserRole<string>()
            {
                UserId = user.Id,
                RoleId = "f5ccae1a-1755-4c72-be25-0c6cd475ea76",
            });

            await this.db.SaveChangesAsync();
        }
    }
}
