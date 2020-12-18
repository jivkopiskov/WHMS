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
    using WHMS.Data.Models;
    using WHMS.Data.Models.Orders;
    using WHMS.Data.Models.Orders.Enum;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;
    using WHMS.Services.Orders;
    using WHMS.Services.Products;
    using WHMS.Web.ViewModels.Orders;
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

        [Fact]
        public async Task ShipOrderShouldUpdateOrderStatusesAndInventory()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var warehouse = new Warehouse
            {
                Address = new Address { },
                Name = "Test",
            };
            var product = new Product
            {
                ProductName = "Test Product",
            };
            var productWarehouse = new ProductWarehouse
            {
                Product = product,
                Warehouse = warehouse,
                AggregateQuantity = 0,
                TotalPhysicalQuanitiy = 10,
                ReservedQuantity = 5,
            };

            var shippingService = "FedEx";
            var carrier = new Carrier { Name = shippingService };
            context.Carriers.Add(carrier);
            var shipping = new ShippingMethod { Carrier = carrier, CarrierId = carrier.Id, Name = shippingService };
            context.ShippingMethods.Add(shipping);
            await context.SaveChangesAsync();
            context.Warehouses.Add(warehouse);
            context.Products.Add(product);
            context.ProductWarehouses.Add(productWarehouse);

            await context.SaveChangesAsync();
            var order = new Order { WarehouseId = warehouse.Id };
            context.Orders.Add(order);
            var orderItem = new OrderItem { ProductId = product.Id, Qty = 5, OrderId = order.Id, Order = order };
            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            var mockInventoryService = new Mock<IInventoryService>();
            var service = new ShippingService(context, mockInventoryService.Object);
            var shipMethod = new ShippingMethodInputModel { CarrierId = carrier.Id, Id = shipping.Id };
            await service.ShipOrderAsync(new Web.ViewModels.Orders.ShipOrderInputModel { OrderId = order.Id, ShippingMethod = shipMethod, TrackingNumber = "test" });
            Assert.True(order.ShippingStatus == ShippingStatus.Shipped);
            Assert.True(order.OrderStatus == OrderStatus.Completed);
            Assert.True(order.TrackingNumber == "test");

            mockInventoryService.Verify(x => x.RecalculateInventoryAfterShippingAsync(It.IsAny<int>(), It.IsAny<int>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task UnhipOrderShouldUpdateOrderStatusesAndInventory()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var warehouse = new Warehouse
            {
                Address = new Address { },
                Name = "Test",
            };
            var product = new Product
            {
                ProductName = "Test Product",
            };
            var productWarehouse = new ProductWarehouse
            {
                Product = product,
                Warehouse = warehouse,
                AggregateQuantity = 0,
                TotalPhysicalQuanitiy = 10,
                ReservedQuantity = 5,
            };

            var shippingService = "FedEx";
            var carrier = new Carrier { Name = shippingService };
            context.Carriers.Add(carrier);
            var shipping = new ShippingMethod { Carrier = carrier, CarrierId = carrier.Id, Name = shippingService };
            context.ShippingMethods.Add(shipping);
            await context.SaveChangesAsync();
            context.Warehouses.Add(warehouse);
            context.Products.Add(product);
            context.ProductWarehouses.Add(productWarehouse);

            await context.SaveChangesAsync();
            var order = new Order { WarehouseId = warehouse.Id, ShippingStatus = ShippingStatus.Shipped, OrderStatus = OrderStatus.Completed };
            context.Orders.Add(order);
            var orderItem = new OrderItem { ProductId = product.Id, Qty = 5, OrderId = order.Id, Order = order };
            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            var mockInventoryService = new Mock<IInventoryService>();
            var service = new ShippingService(context, mockInventoryService.Object);
            var shipMethod = new ShippingMethodInputModel { CarrierId = carrier.Id, Id = shipping.Id };
            await service.UnshipOrderAsync(order.Id);
            Assert.True(order.ShippingStatus == ShippingStatus.Unshipped);
            Assert.True(order.OrderStatus == OrderStatus.Processing);

            mockInventoryService.Verify(x => x.RecalculateInventoryAfterUnshippingAsync(It.IsAny<int>(), It.IsAny<int>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task UnhipOrderShouldNotUpdateOrderStatusesAndInventoryWhenAlreadyUnshipped()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var warehouse = new Warehouse
            {
                Address = new Address { },
                Name = "Test",
            };
            var product = new Product
            {
                ProductName = "Test Product",
            };
            var productWarehouse = new ProductWarehouse
            {
                Product = product,
                Warehouse = warehouse,
                AggregateQuantity = 0,
                TotalPhysicalQuanitiy = 10,
                ReservedQuantity = 5,
            };

            var shippingService = "FedEx";
            var carrier = new Carrier { Name = shippingService };
            context.Carriers.Add(carrier);
            var shipping = new ShippingMethod { Carrier = carrier, CarrierId = carrier.Id, Name = shippingService };
            context.ShippingMethods.Add(shipping);
            await context.SaveChangesAsync();
            context.Warehouses.Add(warehouse);
            context.Products.Add(product);
            context.ProductWarehouses.Add(productWarehouse);

            await context.SaveChangesAsync();
            var order = new Order { WarehouseId = warehouse.Id, ShippingStatus = ShippingStatus.Unshipped, OrderStatus = OrderStatus.Processing };
            context.Orders.Add(order);
            var orderItem = new OrderItem { ProductId = product.Id, Qty = 5, OrderId = order.Id, Order = order };
            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            var mockInventoryService = new Mock<IInventoryService>();
            var service = new ShippingService(context, mockInventoryService.Object);
            var shipMethod = new ShippingMethodInputModel { CarrierId = carrier.Id, Id = shipping.Id };
            await service.UnshipOrderAsync(order.Id);

            mockInventoryService.Verify(x => x.RecalculateInventoryAfterUnshippingAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task ShipOrderShouldNotUpdateOrderStatusesAndInventoryWhenAlreadyShipped()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var warehouse = new Warehouse
            {
                Address = new Address { },
                Name = "Test",
            };
            var product = new Product
            {
                ProductName = "Test Product",
            };
            var productWarehouse = new ProductWarehouse
            {
                Product = product,
                Warehouse = warehouse,
                AggregateQuantity = 0,
                TotalPhysicalQuanitiy = 10,
                ReservedQuantity = 5,
            };

            var shippingService = "FedEx";
            var carrier = new Carrier { Name = shippingService };
            context.Carriers.Add(carrier);
            var shipping = new ShippingMethod { Carrier = carrier, CarrierId = carrier.Id, Name = shippingService };
            context.ShippingMethods.Add(shipping);
            await context.SaveChangesAsync();
            context.Warehouses.Add(warehouse);
            context.Products.Add(product);
            context.ProductWarehouses.Add(productWarehouse);

            await context.SaveChangesAsync();
            var order = new Order { WarehouseId = warehouse.Id, ShippingStatus = ShippingStatus.Shipped };
            context.Orders.Add(order);
            var orderItem = new OrderItem { ProductId = product.Id, Qty = 5, OrderId = order.Id, Order = order };
            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            var mockInventoryService = new Mock<IInventoryService>();
            var service = new ShippingService(context, mockInventoryService.Object);
            var shipMethod = new ShippingMethodInputModel { CarrierId = carrier.Id, Id = shipping.Id };
            await service.ShipOrderAsync(new Web.ViewModels.Orders.ShipOrderInputModel { OrderId = order.Id, ShippingMethod = shipMethod, TrackingNumber = "test" });

            mockInventoryService.Verify(x => x.RecalculateInventoryAfterShippingAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
    }
}
