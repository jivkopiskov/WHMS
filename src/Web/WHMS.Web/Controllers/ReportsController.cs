namespace WHMS.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using WHMS.Services;
    using WHMS.Web.ViewModels.Reports;

    public class ReportsController : Controller
    {
        private readonly IReportServices reportServices;

        public ReportsController(IReportServices reportServices)
        {
            this.reportServices = reportServices;
        }

        public IActionResult Index()
        {
            var model = new ReportIndexViewModel
            {
                SoldToday = this.reportServices.GetQtySoldToday(),
                QtySoldLastXDays = this.reportServices.GetQtySoldLast(7),
            };
            model.QtySoldLastXDays.QtySoldList.OrderBy(x => x.Date);
            return this.View(model);
        }

        public IActionResult SalesComparasion()
        {
            var qtySold = this.reportServices.GetQtySoldPerChannelLast(7).OrderBy(x => x.Date);
            var model = qtySold.GroupBy(x => x.Channel);
            return this.View(model);
        }
    }
}
