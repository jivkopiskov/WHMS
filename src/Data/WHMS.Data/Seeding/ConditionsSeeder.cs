namespace WHMS.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using WHMS.Data.Models.Products;

    public class ConditionsSeeder : ISeeder
    {
        public async Task SeedAsync(WHMSDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.ProductConditions.Any())
            {
                return;
            }

            await dbContext.ProductConditions.AddAsync(new ProductCondition() { Name = "New", Description = "Brand New" });
            await dbContext.ProductConditions.AddAsync(new ProductCondition() { Name = "Used", Description = "Used" });
            await dbContext.ProductConditions.AddAsync(new ProductCondition() { Name = "Refurbished", Description = "Certified refurbsihed" });
            await dbContext.ProductConditions.AddAsync(new ProductCondition() { Name = "Like New", Description = "Like new" });
            await dbContext.ProductConditions.AddAsync(new ProductCondition() { Name = "Good", Description = "Used, no  damage" });
            await dbContext.ProductConditions.AddAsync(new ProductCondition() { Name = "Very Good", Description = "Used, minimal damage" });
        }
    }
}
