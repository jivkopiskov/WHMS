namespace WHMS.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using WHMS.Data.Models.Products;
    using WHMS.Data.Models.PurchaseOrder;

    public class PurchaseItemConfiguration : IEntityTypeConfiguration<PurchaseItem>
    {
        public void Configure(EntityTypeBuilder<PurchaseItem> purchaseItem)
        {
            purchaseItem.HasOne(pi => pi.PurchaseOrder)
                .WithMany(p => p.PurchaseItems);

            purchaseItem.HasOne(pi => pi.Product)
                .WithMany(p => p.PurchaseItems);
        }
    }
}
