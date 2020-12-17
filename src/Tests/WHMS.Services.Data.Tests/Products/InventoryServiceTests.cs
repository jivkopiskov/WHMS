namespace WHMS.Services.Tests.Products
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using WHMS.Data;
    using WHMS.Data.Models;
    using WHMS.Data.Models.Orders;
    using WHMS.Data.Models.Orders.Enum;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Products;
    using WHMS.Web.ViewModels.Products;
    using Xunit;

    public class InventoryServiceTests : BaseServiceTest
    {
        [Fact]
        public async Task GetAvailableInventoryShouldReturnZeroWithZeroPhysicalInventoryInWarehouse()
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
                TotalPhysicalQuanitiy = 0,
                ReservedQuantity = 0,
            };
            context.Warehouses.Add(warehouse);
            context.Products.Add(product);
            context.ProductWarehouses.Add(productWarehouse);
            await context.SaveChangesAsync();

            var service = new InventoryService(context);
            var availableInventory = service.GetProductAvailableInventory(product.Id);
            var expected = 0;

            Assert.Equal(expected, availableInventory);
        }

        [Fact]
        public async Task GetAvailableInventoryShouldReturnZeroWithPositivePhysicalInventoryInWarehouse()
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
                ReservedQuantity = 10,
            };
            context.Warehouses.Add(warehouse);
            context.Products.Add(product);
            context.ProductWarehouses.Add(productWarehouse);
            await context.SaveChangesAsync();

            var service = new InventoryService(context);
            var availableInventory = service.GetProductAvailableInventory(product.Id);
            var expected = 0;

            Assert.Equal(expected, availableInventory);
        }

        [Fact]
        public async Task GetAvailableInventoryShouldReturnPositiveWithPositiveAggregateInventoryInWarehouse()
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
                AggregateQuantity = 10,
                TotalPhysicalQuanitiy = 10,
                ReservedQuantity = 0,
            };
            context.Warehouses.Add(warehouse);
            context.Products.Add(product);
            context.ProductWarehouses.Add(productWarehouse);
            await context.SaveChangesAsync();

            var service = new InventoryService(context);
            var availableInventory = service.GetProductAvailableInventory(product.Id);
            var expected = 10;

            Assert.Equal(expected, availableInventory);
        }

        [Fact]
        public async Task GetAvailableInventoryShouldReturnPositiveWithPositiveAggregateInventoryInMultipleWarehouses()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var warehouse = new Warehouse
            {
                Address = new Address { },
                Name = "Test",
            };
            var warehouse2 = new Warehouse
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
                AggregateQuantity = 5,
                TotalPhysicalQuanitiy = 5,
                ReservedQuantity = 0,
            };
            var productWarehouse2 = new ProductWarehouse
            {
                Product = product,
                Warehouse = warehouse2,
                AggregateQuantity = 12,
                TotalPhysicalQuanitiy = 12,
                ReservedQuantity = 0,
            };
            context.Warehouses.Add(warehouse);
            context.Warehouses.Add(warehouse2);
            context.Products.Add(product);
            context.ProductWarehouses.Add(productWarehouse);
            context.ProductWarehouses.Add(productWarehouse2);
            await context.SaveChangesAsync();

            var service = new InventoryService(context);
            var availableInventory = service.GetProductAvailableInventory(product.Id);
            var expected = 17;

            Assert.Equal(expected, availableInventory);
        }

        [Fact]
        public async Task GetPhysicalInventoryShouldReturnZeroWithZeroPhysicalInventoryInWarehouse()
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
                TotalPhysicalQuanitiy = 0,
                ReservedQuantity = 0,
            };
            context.Warehouses.Add(warehouse);
            context.Products.Add(product);
            context.ProductWarehouses.Add(productWarehouse);
            await context.SaveChangesAsync();

            var service = new InventoryService(context);
            var availableInventory = service.GetProductPhysicalInventory(product.Id);
            var expected = 0;

            Assert.Equal(expected, availableInventory);
        }

        [Fact]
        public async Task GetPhysicalInventoryShouldReturnPositiveWithPositivePhysicalInventoryAndZeroAggregateInWarehouse()
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
                ReservedQuantity = 10,
            };
            context.Warehouses.Add(warehouse);
            context.Products.Add(product);
            context.ProductWarehouses.Add(productWarehouse);
            await context.SaveChangesAsync();

            var service = new InventoryService(context);
            var physicalInventory = service.GetProductPhysicalInventory(product.Id);
            var expected = 10;

            Assert.Equal(expected, physicalInventory);
        }

        [Fact]
        public async Task GetPhysicalInventoryShouldReturnPositiveWithPositiveAggregateInventoryInWarehouse()
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
                AggregateQuantity = 10,
                TotalPhysicalQuanitiy = 10,
                ReservedQuantity = 0,
            };
            context.Warehouses.Add(warehouse);
            context.Products.Add(product);
            context.ProductWarehouses.Add(productWarehouse);
            await context.SaveChangesAsync();

            var service = new InventoryService(context);
            var physicalInventory = service.GetProductPhysicalInventory(product.Id);
            var expected = 10;

            Assert.Equal(expected, physicalInventory);
        }

        [Fact]
        public async Task GetPhysicalInventoryShouldReturnPositiveWithPositiveAggregateInventoryInMultipleWarehouses()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var warehouse = new Warehouse
            {
                Address = new Address { },
                Name = "Test",
            };
            var warehouse2 = new Warehouse
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
                AggregateQuantity = 5,
                TotalPhysicalQuanitiy = 5,
                ReservedQuantity = 0,
            };
            var productWarehouse2 = new ProductWarehouse
            {
                Product = product,
                Warehouse = warehouse2,
                AggregateQuantity = 12,
                TotalPhysicalQuanitiy = 12,
                ReservedQuantity = 0,
            };
            context.Warehouses.Add(warehouse);
            context.Warehouses.Add(warehouse2);
            context.Products.Add(product);
            context.ProductWarehouses.Add(productWarehouse);
            context.ProductWarehouses.Add(productWarehouse2);
            await context.SaveChangesAsync();

            var service = new InventoryService(context);
            var physicalInventory = service.GetProductPhysicalInventory(product.Id);
            var expected = 17;

            Assert.Equal(expected, physicalInventory);
        }

        [Fact]
        public async Task RecalcualteAvaialbleInventoryShouldCreateProductWarehouseIfItDoesntExist()
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

            context.Warehouses.Add(warehouse);
            context.Products.Add(product);

            await context.SaveChangesAsync();

            var service = new InventoryService(context);
            await service.RecalculateAvailableInventoryAsync(product.Id);
            var productWarehouse = context.ProductWarehouses.FirstOrDefault();

            Assert.NotNull(productWarehouse);
            Assert.Equal(0, productWarehouse.AggregateQuantity);
        }

        [Fact]
        public async Task RecalcualteAvaialbleInventoryShouldRecalculateAggregateQtyWithoutReserves()
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
                ReservedQuantity = 0,
            };
            context.Warehouses.Add(warehouse);
            context.Products.Add(product);
            context.ProductWarehouses.Add(productWarehouse);
            await context.SaveChangesAsync();

            var service = new InventoryService(context);
            await service.RecalculateAvailableInventoryAsync(product.Id);
            productWarehouse = context.ProductWarehouses.FirstOrDefault();
            var expected = 10;

            Assert.Equal(expected, productWarehouse.AggregateQuantity);
        }

        [Fact]
        public async Task RecalcualteAvaialbleInventoryShouldRecalculateReservedInventory()
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
            context.Warehouses.Add(warehouse);
            context.Products.Add(product);
            context.ProductWarehouses.Add(productWarehouse);
            await context.SaveChangesAsync();

            var service = new InventoryService(context);
            await service.RecalculateAvailableInventoryAsync(product.Id);
            productWarehouse = context.ProductWarehouses.FirstOrDefault();
            var expected = 0;

            Assert.Equal(expected, productWarehouse.ReservedQuantity);
        }

        [Fact]
        public async Task RecalcualteAvaialbleInventoryShouldRecalculateReservedInventoryWithOrders()
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

            context.Warehouses.Add(warehouse);
            context.Products.Add(product);
            context.ProductWarehouses.Add(productWarehouse);

            await context.SaveChangesAsync();
            var order = new Order { WarehouseId = warehouse.Id };
            context.Orders.Add(order);
            var orderItem = new OrderItem { ProductId = product.Id, Qty = 5, OrderId = order.Id, Order = order };
            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            var service = new InventoryService(context);
            await service.RecalculateAvailableInventoryAsync(product.Id);
            productWarehouse = context.ProductWarehouses.FirstOrDefault();
            var expected = 5;

            Assert.Equal(expected, productWarehouse.ReservedQuantity);
        }

        [Fact]
        public async Task AdjustInventoryShouldAdjustInventoryCorrectlyWithPositiveAdjustment()
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
                ReservedQuantity = 0,
            };
            context.Warehouses.Add(warehouse);
            context.Products.Add(product);
            context.ProductWarehouses.Add(productWarehouse);
            await context.SaveChangesAsync();

            var service = new InventoryService(context);
            var adjustment = new ProductAdjustmentInputModel { ProductId = product.Id, Qty = 10, WarehouseId = warehouse.Id };
            await service.AdjustInventoryAsync(adjustment);

            productWarehouse = context.ProductWarehouses.FirstOrDefault();
            var expected = 20;

            Assert.Equal(expected, productWarehouse.TotalPhysicalQuanitiy);
            Assert.Equal(expected, productWarehouse.AggregateQuantity);
        }

        [Fact]
        public async Task AdjustInventoryShouldAdjustInventoryCorrectlyWithNegativeAdjustment()
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
                ReservedQuantity = 0,
            };
            context.Warehouses.Add(warehouse);
            context.Products.Add(product);
            context.ProductWarehouses.Add(productWarehouse);
            await context.SaveChangesAsync();

            var service = new InventoryService(context);
            var adjustment = new ProductAdjustmentInputModel { ProductId = product.Id, Qty = -10, WarehouseId = warehouse.Id };
            await service.AdjustInventoryAsync(adjustment);

            productWarehouse = context.ProductWarehouses.FirstOrDefault();
            var expected = 0;

            Assert.Equal(expected, productWarehouse.TotalPhysicalQuanitiy);
            Assert.Equal(expected, productWarehouse.AggregateQuantity);
        }

        [Fact]
        public async Task AdjustInventoryShouldAdjustInventoryCorrectlyWhenProductWarehouseDoesntExist()
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

            context.Warehouses.Add(warehouse);
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var service = new InventoryService(context);
            var adjustment = new ProductAdjustmentInputModel { ProductId = product.Id, Qty = 10, WarehouseId = warehouse.Id };
            await service.AdjustInventoryAsync(adjustment);

            var productWarehouse = context.ProductWarehouses.FirstOrDefault();
            var expected = 10;

            Assert.NotNull(productWarehouse);
            Assert.Equal(expected, productWarehouse.AggregateQuantity);
        }

        [Fact]
        public async Task RecalcInventoryAfterShipping()
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

            context.Warehouses.Add(warehouse);
            context.Products.Add(product);
            context.ProductWarehouses.Add(productWarehouse);

            await context.SaveChangesAsync();
            var order = new Order { WarehouseId = warehouse.Id, ShippingStatus = ShippingStatus.Shipped };
            context.Orders.Add(order);
            var orderItem = new OrderItem { ProductId = product.Id, Qty = 5, OrderId = order.Id, Order = order };
            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            var service = new InventoryService(context);
            await service.RecalculateInventoryAfterShippingAsync(product.Id, warehouse.Id);
            productWarehouse = context.ProductWarehouses.FirstOrDefault();
            var expected = 0;
            var expectedPhysical = 5;

            Assert.Equal(expected, productWarehouse.ReservedQuantity);
            Assert.Equal(expectedPhysical, productWarehouse.TotalPhysicalQuanitiy);
        }

        [Fact]
        public async Task RecalcInventoryAfterUnshipping()
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

            context.Warehouses.Add(warehouse);
            context.Products.Add(product);
            context.ProductWarehouses.Add(productWarehouse);

            await context.SaveChangesAsync();
            var order = new Order { WarehouseId = warehouse.Id, ShippingStatus = ShippingStatus.Unshipped };
            context.Orders.Add(order);
            var orderItem = new OrderItem { ProductId = product.Id, Qty = 5, OrderId = order.Id, Order = order };
            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            var service = new InventoryService(context);
            await service.RecalculateInventoryAfterUnshippingAsync(product.Id, warehouse.Id);
            productWarehouse = context.ProductWarehouses.FirstOrDefault();
            var expected = 5;
            var expectedPhysical = 15;

            Assert.Equal(expected, productWarehouse.ReservedQuantity);
            Assert.Equal(expectedPhysical, productWarehouse.TotalPhysicalQuanitiy);
        }
    }
}
