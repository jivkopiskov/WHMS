namespace WHMS.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using WHMS.Common;
    using WHMS.Data.Models;

    public class AdminSeeder : ISeeder
    {
        public async Task SeedAsync(WHMSDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            await SeedAdminAsync(userManager, configuration, GlobalConstants.AdministratorRoleName);
        }

        private static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, IConfiguration configuration, string roleName)
        {
            var username = configuration["Admin:Email"];
            var password = configuration["Admin:Password"];

            var user = await userManager.FindByEmailAsync(username);
            if (user == null)
            {
                user = new ApplicationUser { Email = username, EmailConfirmed = true, IsApproved = true, UserName = username };
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
