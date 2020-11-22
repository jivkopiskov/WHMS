namespace WHMS.Web.ViewModels.PurchaseOrders
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Models.PurchaseOrders;
    using WHMS.Services.Mapping;
    using WHMS.Web.ViewModels.Products;

    public class AddPurchaseOrderInputModel : IMapTo<PurchaseOrder>
    {
        [Required]
        [MaxLength(100)]
        public string Description { get; set; }

        public int WarehouseId { get; set; }

        public DateTime? ETA { get; set; }

        [Range(1, double.MaxValue)]
        public int VendorId { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "Shipping charges")]
        public decimal ShippingFee { get; set; }
    }
}
