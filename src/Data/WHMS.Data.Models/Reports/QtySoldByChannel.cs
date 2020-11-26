namespace WHMS.Data.Models.Reports
{
    using System;

    using WHMS.Data.Models.Orders.Enum;

    public class QtySoldByChannel
    {
        public int Id { get; set; }

        public Channel Channel { get; set; }

        public DateTime Date { get; set; }

        public int QtySold { get; set; }

        public decimal AmountSold { get; set; }
    }
}
