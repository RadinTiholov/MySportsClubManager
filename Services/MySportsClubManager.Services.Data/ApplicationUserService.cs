namespace MySportsClubManager.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using MySportsClubManager.Common;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.ApplicationUser;

    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> applicationUserRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public ApplicationUserService(IDeletableEntityRepository<ApplicationUser> applicationUserRepository, UserManager<ApplicationUser> userManager)
        {
            this.applicationUserRepository = applicationUserRepository;
            this.userManager = userManager;
        }

        public async Task<List<ApplicationUserInformationViewModel>> AllAsync()
        {
            return await this.applicationUserRepository.All()
                .Include(x => x.Roles)
                .To<ApplicationUserInformationViewModel>()
                .ToListAsync();
        }

        public async Task AssignUserToRoleAsync(string id, string roleName)
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

            var result = await this.userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<string> GetCurrentUserProfilePicAsync(string id)
        {
            string imageUrl = await this.applicationUserRepository.All()
                .Where(u => u.Id == id)
                .Include(u => u.Image)
                .Select(u => u.Image.URL)
                .FirstOrDefaultAsync();

            return imageUrl;
        }

        public async Task RemoveUserFromRoleAsync(string id)
        {
            var user = await this.applicationUserRepository.All()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new InvalidOperationException(ServiceErrorMessages.UserNotFoundMessage);
            }

            if (await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName))
            {
                var result = await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.AdministratorRoleName);
                return;
            }

            if (await this.userManager.IsInRoleAsync(user, GlobalConstants.TrainerRoleName))
            {
                var result = await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.TrainerRoleName);
                return;
            }

            throw new InvalidOperationException(ServiceErrorMessages.UserIsNotInRoleMessage);
        }
    }
}
