namespace MySportsClubManager.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using MySportsClubManager.Data;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Data.Repositories;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.ApplicationUser;
    using MySportsClubManager.Web.ViewModels.Contest;
    using Xunit;

    public class ApplicationUserServiceTests
    {
        private IDeletableEntityRepository<ApplicationUser> applicationUserRepository;
        private UserManager<ApplicationUser> userManager;
        private ApplicationDbContext applicationDbContext;
        private ApplicationUserService applicationUserService;

        public ApplicationUserServiceTests()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MySportsClubManagerDbUsers")
            .Options;

            this.applicationDbContext = new ApplicationDbContext(contextOptions);

            this.applicationDbContext.Database.EnsureDeleted();
            this.applicationDbContext.Database.EnsureCreated();

            this.applicationUserRepository = new EfDeletableEntityRepository<ApplicationUser>(this.applicationDbContext);
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns(async (ApplicationUser user, string role) =>
                {
                    await Task.Delay(1);
                    if (user.Id == "TestId")
                    {
                        return false;
                    }

                    return true;
                });
            userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns(async () =>
                {
                    await Task.Delay(1);
                    await this.applicationDbContext.Roles.AddAsync(new ApplicationRole() { Name = "Administration", Id = "RoleId" });
                    await this.applicationDbContext.UserRoles.AddAsync(new IdentityUserRole<string>() { UserId = "TestId", RoleId = "RoleId" });
                    await this.applicationDbContext.SaveChangesAsync();
                    return new IdentityResult();
                });
            userManagerMock.Setup(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns(async (ApplicationUser user, string roleId) =>
                {
                    await Task.Delay(1);
                    var userRole = await this.applicationDbContext.UserRoles.Where(x => x.UserId == user.Id && x.RoleId == "RoleId").FirstOrDefaultAsync();
                    this.applicationDbContext.UserRoles.Remove(userRole);
                    await this.applicationDbContext.SaveChangesAsync();
                    return new IdentityResult();
                });
            this.userManager = userManagerMock.Object;
            this.applicationUserService = new ApplicationUserService(this.applicationUserRepository, this.userManager);
        }

        [Fact]
        public async Task AllAsyncShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ApplicationUserInformationViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            var users = await this.applicationUserService.AllAsync();

            Assert.Equal(2, users.Count);
        }

        [Fact]
        public async Task GetCurrentUserProfilePicAsyncShouldWorkCorrectly()
        {
            this.SeedData();

            var url = await this.applicationUserService.GetCurrentUserProfilePicAsync("TestId");

            Assert.Equal("aaa.com", url);
        }

        [Fact]
        public async Task GetProfileInformationAsyncShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ProfileViewModel).GetTypeInfo().Assembly);
            this.SeedData();

            var information = await this.applicationUserService.GetProfileInformationAsync("TestId");

            Assert.Equal("aaa.com", information.ImageUrl);
            Assert.Equal("Test", information.FirstName);
            Assert.Equal("Test", information.UserName);
        }

        [Fact]
        public async Task AssignUserToRoleAsyncShouldWorkCorrectly()
        {
            this.SeedData();

            await this.applicationUserService.AssignUserToRoleAsync("TestId", "Administration");
            var user = await this.applicationDbContext.Users.Where(x => x.Id == "TestId").Include(x => x.Roles).FirstOrDefaultAsync();
            Assert.Equal(1, user.Roles.Count);
        }

        [Fact]
        public async Task AssignUserToRoleAsyncShouldThrowExWhenNotFound()
        {
            this.SeedData();

            var url = await this.applicationUserService.GetCurrentUserProfilePicAsync("TestId");

            await Assert.ThrowsAsync<InvalidOperationException>(async () => { await this.applicationUserService.AssignUserToRoleAsync("TestId4", "Administration"); });
        }

        [Fact]
        public async Task AssignUserToRoleAsyncShouldThrowExWhenIsAlreadyInRole()
        {
            this.SeedData();

            var url = await this.applicationUserService.GetCurrentUserProfilePicAsync("TestId");

            await Assert.ThrowsAsync<InvalidOperationException>(async () => { await this.applicationUserService.AssignUserToRoleAsync("TestId2", "Administration"); });
        }

        [Fact]
        public async Task RemoveUserFromRoleAsyncShouldWorkCorrectly()
        {
            this.SeedData();

            await this.applicationDbContext.Roles.AddAsync(new ApplicationRole() { Name = "Administration", Id = "RoleId" });
            await this.applicationDbContext.UserRoles.AddAsync(new IdentityUserRole<string>() { UserId = "TestId2", RoleId = "RoleId" });
            await this.applicationDbContext.SaveChangesAsync();
            await this.applicationUserService.RemoveUserFromRoleAsync("TestId2");
            var user = await this.applicationDbContext.Users.Where(x => x.Id == "TestId2").Include(x => x.Roles).FirstOrDefaultAsync();
            Assert.Equal(0, user.Roles.Count);
        }

        [Fact]
        public async Task RemoveUserFromRoleAsyncShouldThrowExWhenNotInRole()
        {
            this.SeedData();

            await Assert.ThrowsAsync<InvalidOperationException>(async () => { await this.applicationUserService.RemoveUserFromRoleAsync("TestId"); });
        }

        [Fact]
        public async Task RemoveUserFromRoleAsyncShouldThrowExWhenNotFound()
        {
            this.SeedData();

            await Assert.ThrowsAsync<InvalidOperationException>(async () => { await this.applicationUserService.RemoveUserFromRoleAsync("TestId5"); });
        }

        private async void SeedData()
        {
            var image = new Image()
            {
                Id = 1,
                URL = "aaa.com",
            };
            var user = new ApplicationUser()
            {
                Id = "TestId",
                UserName = "Test",
                FirstName = "Test",
                LastName = "Testov",
                Email = "TestEmail",
                Image = image,
            };
            var user2 = new ApplicationUser()
            {
                Id = "TestId2",
                UserName = "Test",
                FirstName = "Test",
                LastName = "Testov",
                Email = "TestEmail",
                Image = image,
            };

            await this.applicationDbContext.Users.AddAsync(user);
            await this.applicationDbContext.Users.AddAsync(user2);
            await this.applicationDbContext.SaveChangesAsync();
        }
    }
}
