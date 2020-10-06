namespace WHMS.Data.Models.PurchaseOrders
{
    using WHMS.Data.Common.Models;
    using WHMS.Data.Models.Products;

    public class PurchaseItem
    {
        public int PurchaseOrderId { get; set; }

        public PurchaseOrder PurchaseOrder { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Qty { get; set; }

        public decimal Cost { get; set; }
    }
}
