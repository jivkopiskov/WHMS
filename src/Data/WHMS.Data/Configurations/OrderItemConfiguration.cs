namespace WHMS.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using WHMS.Data.Models.Orders;
    using WHMS.Data.Models.Products;

    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> orderItem)
        {
            orderItem.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems);

            orderItem.HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems);
        }
    }
}
