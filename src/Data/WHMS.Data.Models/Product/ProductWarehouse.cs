namespace WHMS.Data.Models.Products
{
    public class ProductWarehouse
    {
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int WarehouseId { get; set; }

        public Warehouse Warehouse { get; set; }

        public int TotalPhysicalQuanitiy { get; set; }

        public int AggregateQuantity { get; set; }

        public int ReservedQuantity { get; set; }
    }
}
