namespace WHMS.Data.Models.PurchaseOrder
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Common.Models;
    using WHMS.Data.Models.Products;
    using WHMS.Data.Models.PurchaseOrder.Enum;

    public class PurchaseOrder : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(100)]
        public string Description { get; set; }

        public int WarehouseId { get; set; }

        public Warehouse Warehouse { get; set; }

        public DateTime? ETA { get; set; }

        public int VendorId { get; set; }

        public Vendor Vendor { get; set; }

        public ICollection<PurchaseItem> PurchaseItems { get; set; } = new HashSet<PurchaseItem>();

        public PurchaseOrderStatus PurchaseOrderStatus { get; set; }

        public ReceivingStatus ReceivingStatus { get; set; }

        public decimal ShippingFee { get; set; }

        public decimal GrandTotal { get; set; }

        public string CreatedById { get; set; }

        public ApplicationUser CreatedBy { get; set; }
    }
}
