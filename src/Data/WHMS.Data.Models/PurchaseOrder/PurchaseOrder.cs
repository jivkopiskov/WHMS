namespace WHMS.Data.Models.PurchaseOrder
{
    using System;
    using System.Collections.Generic;

    using WHMS.Data.Common.Models;
    using WHMS.Data.Models.Product;
    using WHMS.Data.Models.PurchaseOrder.Enum;

    public class PurchaseOrder : BaseDeletableModel<int>
    {
        public int WarehouseId { get; set; }

        public Warehouse Warehouse { get; set; }

        public DateTime? ETA { get; set; }

        public Vendor Vendor { get; set; }

        public ICollection<PurchaseItem> PurchaseItems { get; set; } = new HashSet<PurchaseItem>();

        public PurchaseOrderStatus PurchaseOrderStatus { get; set; }

        public ReceivingStatus ReceivingStatus { get; set; }

        public decimal ShippingFee { get; set; }

        public decimal GrandTotal { get; set; }
    }
}
