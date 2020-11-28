namespace WHMS.Web.ViewModels.Reports
{
    using System;

    using WHMS.Data.Models.Reports;
    using WHMS.Services.Mapping;

    public class QtySoldViewModel : IMapFrom<QtySoldByDay>
    {
        public DateTime Date { get; set; }

        public int QtySold { get; set; }

        public decimal AmountSold { get; set; }
    }
}
