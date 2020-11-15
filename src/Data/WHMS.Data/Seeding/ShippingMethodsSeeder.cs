namespace WHMS.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using WHMS.Data.Models.Orders;

    public class ShippingMethodsSeeder : ISeeder
    {
        public async Task SeedAsync(WHMSDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.ShippingMethods.Any())
            {
                return;
            }

            var ups = dbContext.Carriers.First(x => x.Name == "UPS");
            await dbContext.ShippingMethods.AddAsync(new ShippingMethod() { Name = "Ground", Carrier = ups });
            await dbContext.ShippingMethods.AddAsync(new ShippingMethod() { Name = "Next day air", Carrier = ups });
            await dbContext.ShippingMethods.AddAsync(new ShippingMethod() { Name = "2nd day air", Carrier = ups });
            await dbContext.ShippingMethods.AddAsync(new ShippingMethod() { Name = "International", Carrier = ups });

            var fedex = dbContext.Carriers.First(x => x.Name == "FedEx");
            await dbContext.ShippingMethods.AddAsync(new ShippingMethod() { Name = "Ground", Carrier = fedex });
            await dbContext.ShippingMethods.AddAsync(new ShippingMethod() { Name = "2Day", Carrier = fedex });
            await dbContext.ShippingMethods.AddAsync(new ShippingMethod() { Name = "Standard Overnight", Carrier = fedex });
            await dbContext.ShippingMethods.AddAsync(new ShippingMethod() { Name = "Express", Carrier = fedex });
            await dbContext.ShippingMethods.AddAsync(new ShippingMethod() { Name = "International economy", Carrier = fedex });
            await dbContext.ShippingMethods.AddAsync(new ShippingMethod() { Name = "International priority", Carrier = fedex });

            var dhl = dbContext.Carriers.First(x => x.Name == "DHL");
            await dbContext.ShippingMethods.AddAsync(new ShippingMethod() { Name = "Express", Carrier = dhl });
            await dbContext.ShippingMethods.AddAsync(new ShippingMethod() { Name = "SameDay", Carrier = dhl });
            await dbContext.ShippingMethods.AddAsync(new ShippingMethod() { Name = "Express Worldwide", Carrier = dhl });
        }
    }
}
