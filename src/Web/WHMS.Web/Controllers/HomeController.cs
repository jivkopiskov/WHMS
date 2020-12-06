namespace WHMS.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using WHMS.Services;
    using WHMS.Web.ViewModels;

    public class HomeController : BaseController
    {
        private readonly IReportServices reportServices;

        public HomeController(IReportServices report)
        {
            this.reportServices = report;
        }

        public IActionResult Index()
        {
            if (this.HttpContext.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction(nameof(this.Dashboard));
            }

            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            var salesToday = this.reportServices.GetQtySoldToday();
            var salesLast7 = this.reportServices.GetQtySoldLast(7).QtySoldList;

            var model = new HomeViewModel
            {
                AmountSoldToday = this.reportServices.GetQtySoldToday().AmountSold,
                AmountSoldLast7 = salesLast7.Sum(x => x.AmountSold),
                QtySoldToday = this.reportServices.GetQtySoldToday().QtySold,
                QtySoldLast7 = salesLast7.Sum(x => x.QtySold),
            };
            return this.View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
