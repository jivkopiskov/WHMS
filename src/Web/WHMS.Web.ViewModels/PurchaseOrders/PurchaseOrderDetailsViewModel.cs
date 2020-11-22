namespace WHMS.Web.ViewModels.PurchaseOrders
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using WHMS.Data.Models.PurchaseOrder;
    using WHMS.Data.Models.PurchaseOrder.Enum;
    using WHMS.Services.Mapping;
    using WHMS.Web.ViewModels.Products;

    public class PurchaseOrderDetailsViewModel : IMapFrom<PurchaseOrder>
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int WarehouseId { get; set; }

        public WarehouseViewModel Warehouse { get; set; }

        public DateTime? ETA { get; set; }

        [Display(Name = "Vendor")]
        public VendorViewModel Vendor { get; set; }

        public IEnumerable<PurchaseItemViewModel> PurchaseItems { get; set; }

        [Display(Name = "PO Status")]
        public PurchaseOrderStatus? PurchaseOrderStatus { get; set; }

        [Display(Name = "Receiving Status")]
        public ReceivingStatus? ReceivingStatus { get; set; }

        [Display(Name = "Shipping charges")]
        public decimal ShippingFee { get; set; }

        [Display(Name = "Grand total")]
        public decimal GrandTotal { get; set; }

        public string CreatedByEmail { get; set; }
    }
}
