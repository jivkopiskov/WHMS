namespace WHMS.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using WHMS.Data.Models.Products;

    public class ProductWarehouseConfiguration : IEntityTypeConfiguration<ProductWarehouse>
    {
        public void Configure(EntityTypeBuilder<ProductWarehouse> productWarehouse)
        {
            productWarehouse.HasOne(pw => pw.Product)
                .WithMany(p => p.ProductWarehouses);

            productWarehouse.HasOne(pw => pw.Warehouse)
                .WithMany(w => w.ProductWarehouses);

            productWarehouse.HasKey(x => new { x.WarehouseId, x.ProductId, });
        }
    }
}
