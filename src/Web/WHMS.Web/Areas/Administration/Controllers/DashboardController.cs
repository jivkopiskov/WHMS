namespace WHMS.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    using WHMS.Common;
    using WHMS.Services;
    using WHMS.Web.ViewModels.Administration.Dashboard;

    public class DashboardController : AdministrationController
    {
        private readonly IUsersService usersService;

        public DashboardController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await this.usersService.GetAllUsersAsync();
            return this.View(model);
        }

        public async Task<IActionResult> ApproveEmployee(string email)
        {
            await this.usersService.ApproveUserAsync(email);
            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> AddEmployeeToAdministratorRole(string email)
        {
            await this.usersService.AddToAdminRoleAsync(email);
            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
