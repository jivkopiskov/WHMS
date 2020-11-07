namespace WHMS.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using WHMS.Data.Models.Orders;
    using WHMS.Data.Models.Products;

    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> customer)
        {
            customer.HasIndex(p => p.Email)
                .IsUnique();
        }
    }
}
