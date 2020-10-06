namespace WHMS.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using WHMS.Data.Models.Products;

    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> product)
        {
            product.HasIndex(p => p.SKU)
                .IsUnique();

            product.HasOne(p => p.CreatedBy)
                .WithMany(u => u.Products);
        }
    }
}
