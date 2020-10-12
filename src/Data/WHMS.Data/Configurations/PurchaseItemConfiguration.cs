﻿namespace WHMS.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using WHMS.Data.Models.Products;
    using WHMS.Data.Models.PurchaseOrders;

    public class PurchaseItemConfiguration : IEntityTypeConfiguration<PurchaseItem>
    {
        public void Configure(EntityTypeBuilder<PurchaseItem> purchaseItem)
        {
            purchaseItem.HasKey(x => new { x.PurchaseOrderId, x.ProductId });

            purchaseItem.HasOne(pi => pi.PurchaseOrder)
                .WithMany(p => p.PurchaseItems);

            purchaseItem.HasOne(pi => pi.Product)
                .WithMany(p => p.PurchaseItems);
        }
    }
}