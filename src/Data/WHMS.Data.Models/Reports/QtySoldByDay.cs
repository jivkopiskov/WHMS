namespace WHMS.Data.Models.Reports
{
    using System;

    public class QtySoldByDay
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int QtySold { get; set; }

        public decimal AmountSold { get; set; }
    }
}
