namespace WHMS.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using WHMS.Web.ViewModels.Administration.Dashboard;

    public class DashboardController : AdministrationController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
