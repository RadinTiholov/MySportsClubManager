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
    using MySportsClubManager.Data.Repositories;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.ApplicationUser;

    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> applicationUserRepository;
        private readonly IRepository<ApplicationRole> applicationRoleRepository;
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public ApplicationUserService(IDeletableEntityRepository<ApplicationUser> applicationUserRepository, ApplicationDbContext db, UserManager<ApplicationUser> userManager, IRepository<ApplicationRole> applicationRoleRepository)
        {
            this.applicationUserRepository = applicationUserRepository;
            this.applicationRoleRepository = applicationRoleRepository;
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<List<ApplicationUserInformationViewModel>> AllAsync()
        {
            return await this.applicationUserRepository.All()
                .Include(x => x.Roles)
                .To<ApplicationUserInformationViewModel>()
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

            var role = await this.applicationRoleRepository.All().Where(x => x.Name == roleName).FirstOrDefaultAsync();
            await this.db.UserRoles.AddAsync(new IdentityUserRole<string>()
            {
                UserId = user.Id,
                RoleId = role.Id,
            });

            await this.db.SaveChangesAsync();
        }

        public async Task<string> GetCurrentUserProfilePic(string id)
        {
            string imageUrl = await this.applicationUserRepository.All()
                .Where(u => u.Id == id)
                .Include(u => u.Image)
                .Select(u => u.Image.URL)
                .FirstOrDefaultAsync();

            return imageUrl;
        }
    }
}
