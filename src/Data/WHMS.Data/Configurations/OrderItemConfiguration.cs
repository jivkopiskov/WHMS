namespace WHMS.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using WHMS.Data.Models.Order;
    using WHMS.Data.Models.Product;

    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> orderItem)
        {
            orderItem.HasKey(x => new { x.OrderId, x.ProductId });

            orderItem.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems);

            orderItem.HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems);
        }
    }
}
