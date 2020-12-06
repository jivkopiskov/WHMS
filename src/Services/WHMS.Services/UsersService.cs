namespace WHMS.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using WHMS.Common;
    using WHMS.Data;
    using WHMS.Data.Models;
    using WHMS.Web.ViewModels.Administration.Dashboard;

    public class UsersService : IUsersService
    {
        private readonly WHMSDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersService(WHMSDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task AddToAdminRoleAsync(string email)
        {
            var employee = await this.userManager.FindByEmailAsync(email);
            await this.userManager.AddToRoleAsync(employee, GlobalConstants.AdministratorRoleName);
        }

        public async Task ApproveUserAsync(string email)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            if (user != null)
            {
                user.IsApproved = true;
                await this.context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetAllUsersAsync()
        {
            var users = this.context.Users.Select(x => new EmployeeViewModel
            {
                Email = x.Email,
                IsApproved = x.IsApproved,
            }).ToList();

            foreach (var user in users)
            {
                var employee = await this.userManager.FindByEmailAsync(user.Email);
                user.IsAdmin = await this.userManager.IsInRoleAsync(employee, GlobalConstants.AdministratorRoleName);
            }

            return users;
        }

        public bool IsApproved(string email)
        {
            return this.context.Users.FirstOrDefault(x => x.Email == email)?.IsApproved ?? false;
        }
    }
}
