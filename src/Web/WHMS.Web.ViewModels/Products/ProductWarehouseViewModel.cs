namespace WHMS.Web.ViewModels.Products
{
    public class ProductWarehouseViewModel
    {
        public int ProductId { get; set; }

        public string ProductSKU { get; set; }

        public bool WarehouseIsSellable { get; set; }

        public string WarehouseName { get; set; }

        public int TotalPhysicalQuanitity { get; set; }

        public int AggregateQuantity { get; set; }

        public int ReservedQuantity { get; set; }
    }
}
