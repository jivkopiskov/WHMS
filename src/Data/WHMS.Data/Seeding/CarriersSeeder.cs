namespace WHMS.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using WHMS.Data.Models.Orders;

    public class CarriersSeeder : ISeeder
    {
        public async Task SeedAsync(WHMSDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Carriers.Any())
            {
                return;
            }

            await dbContext.Carriers.AddAsync(new Carrier() { Name = "FedEx" });
            await dbContext.Carriers.AddAsync(new Carrier() { Name = "UPS" });
            await dbContext.Carriers.AddAsync(new Carrier() { Name = "DHL" });
        }
    }
}
