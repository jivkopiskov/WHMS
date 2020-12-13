namespace WHMS.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using WHMS.Data.Models;
    using WHMS.Data.Models.Products;

    public class WarehouseSeeder : ISeeder
    {
        public async Task SeedAsync(WHMSDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Warehouses.Any())
            {
                return;
            }

            await dbContext.Warehouses.AddAsync(new Warehouse() { Name = "Default Warehouse", Address = new Address { StreetAddress = "Default Warehouse" }, IsSellable = true });

        }
    }
}
