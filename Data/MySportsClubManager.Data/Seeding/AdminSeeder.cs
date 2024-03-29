﻿namespace MySportsClubManager.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using MySportsClubManager.Common;
    using MySportsClubManager.Data.Models;

    public class AdminSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Users.Any(x => x.UserName == "admin"))
            {
                return;
            }

            var user = new ApplicationUser
            {
                FirstName = "admin",
                LastName = "adminov",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                PhoneNumber = "+111111111111",
                Image = new Image() { URL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTfTAFxRa7QIR39ejo9tL0zH0-EadbkcrziSSrdA5IrHEGIDfAFZybEvaukbxyHCy4lXJI&usqp=CAU" },
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };

            var password = new PasswordHasher<ApplicationUser>();
            var hashed = password.HashPassword(user, "asdasd");
            user.PasswordHash = hashed;

            var userStore = new UserStore<ApplicationUser>(dbContext);
            var result = await userStore.CreateAsync(user);
            var createdUser = await dbContext.Users.Where(x => x.UserName == "admin").FirstOrDefaultAsync();
            await this.AssignToRole(dbContext, createdUser);
            await this.AssignAthleteAndTrainer(dbContext, createdUser);
        }

        private async Task AssignAthleteAndTrainer(ApplicationDbContext dbContext, ApplicationUser createdUser)
        {
            var athlete = new Athlete()
            {
                ApplicationUserId = createdUser.Id,
            };

            var trainer = new Trainer()
            {
                ApplicationUserId = createdUser.Id,
            };

            await dbContext.Athletes.AddAsync(athlete);
            await dbContext.Trainers.AddAsync(trainer);
        }

        private async Task AssignToRole(ApplicationDbContext dbContext, ApplicationUser createdUser)
        {
            var adminRole = await dbContext.Roles.Where(x => x.Name == GlobalConstants.AdministratorRoleName).FirstOrDefaultAsync();
            await dbContext.UserRoles.AddAsync(new IdentityUserRole<string>()
            {
                UserId = createdUser.Id,
                RoleId = adminRole.Id,
            });
        }
    }
}
