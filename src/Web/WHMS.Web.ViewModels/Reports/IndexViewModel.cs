namespace WHMS.Web.ViewModels.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ReportIndexViewModel
    {
        public QtySoldViewModel SoldToday { get; set; }

        public QtySoldLastViewModel QtySoldLastXDays { get; set; }
    }
}
