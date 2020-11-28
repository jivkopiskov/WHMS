namespace WHMS.Web.ViewModels.Reports
{
    using System;

    using WHMS.Data.Models.Orders.Enum;
    using WHMS.Data.Models.Reports;
    using WHMS.Services.Mapping;

    public class QtySoldByChannelViewModel : IMapFrom<QtySoldByChannel>
    {
        public Channel Channel { get; set; }

        public DateTime Date { get; set; }

        public int QtySold { get; set; }

        public decimal AmountSold { get; set; }
    }
}
