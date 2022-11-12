using MySportsClubManager.Common;
using MySportsClubManager.Data.Models;
using System;
using System.Threading.Tasks;

namespace MySportsClubManager.Data.Seeding
{
    public class TrainerSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            await dbContext.Roles.AddAsync(new ApplicationRole()
            {
                Name = GlobalConstants.TrainerRoleName,
                NormalizedName = GlobalConstants.TrainerRoleName.ToUpper(),
            });
            await dbContext.SaveChangesAsync();
        }
    }
}
