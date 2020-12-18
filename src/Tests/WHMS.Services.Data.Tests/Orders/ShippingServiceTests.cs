namespace WHMS.Services.Tests.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using WHMS.Data;
    using WHMS.Data.Models.Orders;
    using WHMS.Services.Mapping;
    using WHMS.Services.Orders;
    using WHMS.Services.Products;
    using Xunit;

    public class ShippingServiceTests : BaseServiceTest
    {
        [Fact]
        public async Task AddCarrierShouldCreateNewCarrierIfItDoesntExist()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var mockInventoryService = new Mock<IInventoryService>();
            var service = new ShippingService(context, mockInventoryService.Object);

            var carrierName = "FedEx";
            await service.AddCarrierAsync(carrierName);

            var carrierDb = context.Carriers.FirstOrDefault();

            Assert.NotNull(carrierDb);
            Assert.Equal(carrierName, carrierDb.Name);
        }

        [Fact]
        public async Task AddCarrierShouldNotCreateNewCarrierIfExist()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var mockInventoryService = new Mock<IInventoryService>();
            var service = new ShippingService(context, mockInventoryService.Object);
            var carrierName = "FedEx";
            var carrier = new Carrier { Name = carrierName };
            context.Carriers.Add(carrier);
            await context.SaveChangesAsync();

            await service.AddCarrierAsync(carrierName);

            var carrierDb = context.Carriers.FirstOrDefault();

            Assert.Equal(carrierName, carrierDb.Name);
            Assert.Equal(carrier.Id, carrierDb.Id);
        }

        [Fact]
        public async Task AddShippingMethodShouldCreateNewMethodIfItDoesntExist()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var mockInventoryService = new Mock<IInventoryService>();
            var service = new ShippingService(context, mockInventoryService.Object);
            var shippingService = "FedEx";
            var carrier = new Carrier { Name = shippingService };
            context.Carriers.Add(carrier);
            await context.SaveChangesAsync();

            await service.AddShippingMethodAsync(carrier.Id, shippingService);

            var shippingMethod = context.ShippingMethods.FirstOrDefault();

            Assert.NotNull(shippingMethod);
            Assert.Equal(shippingService, shippingMethod.Name);
        }

        [Fact]
        public async Task AddShippingMethodShouldNotCreateNewMethodIfExist()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var mockInventoryService = new Mock<IInventoryService>();
            var service = new ShippingService(context, mockInventoryService.Object);
            var shippingService = "FedEx";
            var carrier = new Carrier { Name = shippingService };
            context.Carriers.Add(carrier);
            var shipping = new ShippingMethod { Carrier = carrier, CarrierId = carrier.Id, Name = shippingService };
            context.ShippingMethods.Add(shipping);
            await context.SaveChangesAsync();

            var id = await service.AddShippingMethodAsync(carrier.Id, shippingService);

            var shippingMethodDB = context.ShippingMethods.FirstOrDefault();

            Assert.Equal(shippingService, shippingMethodDB.Name);
            Assert.Equal(id, shippingMethodDB.Id);
        }

        [Fact]
        public async Task DeleteShippingMethodShouldDelete()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var mockInventoryService = new Mock<IInventoryService>();
            var service = new ShippingService(context, mockInventoryService.Object);
            var shippingService = "FedEx";
            var carrier = new Carrier { Name = shippingService };
            context.Carriers.Add(carrier);
            var shipping = new ShippingMethod { Carrier = carrier, CarrierId = carrier.Id, Name = shippingService };
            context.ShippingMethods.Add(shipping);
            await context.SaveChangesAsync();

            await service.DeleteShippingMethodAsync(shipping.Id);

            var shippingMethodDB = context.ShippingMethods.FirstOrDefault();

            Assert.Null(shippingMethodDB);

        }
    }
}
