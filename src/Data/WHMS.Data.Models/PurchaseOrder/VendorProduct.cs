namespace WHMS.Data.Models.PurchaseOrder
{
    using WHMS.Data.Common.Models;
    using WHMS.Data.Models.Products;
    using WHMS.Data.Models.PurchaseOrder;

    public class VendorProduct : BaseDeletableModel<int>
    {
        public int VendorId { get; set; }

        public Vendor Vendor { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public decimal VendorCost { get; set; }
    }
}
