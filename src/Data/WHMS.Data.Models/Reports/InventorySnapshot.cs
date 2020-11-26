namespace WHMS.Data.Models.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class InventorySnapshot
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public DateTime Date { get; set; }

        public int InventoryAvailable { get; set; }

        public int TotalPhysicalQuanitiy { get; set; }

        public int AggregateQuantity { get; set; }

        public int ReservedQuantity { get; set; }
    }
}
