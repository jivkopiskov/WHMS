namespace WHMS.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;

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

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
