namespace WHMS.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using WHMS.Web.ViewModels.Administration.Dashboard;

    public interface IUsersService
    {
        bool IsApproved(string email);

        Task ApproveUserAsync(string email);

        Task AddToAdminRoleAsync(string email);

        Task<IEnumerable<EmployeeViewModel>> GetAllUsersAsync();
    }
}
