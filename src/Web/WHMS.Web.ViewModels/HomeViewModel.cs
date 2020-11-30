namespace WHMS.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class HomeViewModel
    {
        public decimal AmountSoldToday { get; set; }

        public decimal AmountSoldLast7 { get; set; }

        public decimal QtySoldToday { get; set; }

        public decimal QtySoldLast7 { get; set; }
    }
}
