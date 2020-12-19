namespace WHMS.Services.Tests.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using WHMS.Data;
    using WHMS.Data.Models;
    using WHMS.Data.Models.Orders;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Orders;
    using WHMS.Services.Products;
    using WHMS.Web.ViewModels.Orders;
    using WHMS.Web.ViewModels.Products;
    using Xunit;

    public class OrderItemsServiceTests : BaseServiceTest
    {
        [Fact]
        public async Task AddOrderItemsShouldAddNewItemsToOrder()
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
            var order = new Order { WarehouseId = warehouse.Id };
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var mockInventoryService = new Mock<IInventoryService>();
            var mockOrdersService = new Mock<IOrdersService>();
            var service = new OrderItemsService(context, mockInventoryService.Object, mockOrdersService.Object);
            var model = new AddOrderItemsInputModel { OrderId = order.Id, OrderItems = new List<AddOrderItemViewModel> { new AddOrderItemViewModel { ProductId = product.Id, Price = 100, Qty = 10 } } };

            var id = await service.AddOrderItemAsync(model);

            var orderItemDB = context.OrderItems.FirstOrDefault();

            Assert.NotNull(orderItemDB);
            Assert.Equal(order.Id, id);
            Assert.True(orderItemDB.OrderId == order.Id);
            Assert.True(orderItemDB.ProductId == product.Id);
        }

        [Fact]
        public async Task AddOrderItemsShouldUpdateQtyIfItemsIsAlreadyAdded()
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
            var order = new Order { WarehouseId = warehouse.Id };
            context.Orders.Add(order);
            var orderItem = new OrderItem { OrderId = order.Id, ProductId = product.Id, Qty = 3 };
            context.Orders.Add(order);
            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            var mockInventoryService = new Mock<IInventoryService>();
            var mockOrdersService = new Mock<IOrdersService>();
            var service = new OrderItemsService(context, mockInventoryService.Object, mockOrdersService.Object);
            var model = new AddOrderItemsInputModel { OrderId = order.Id, OrderItems = new List<AddOrderItemViewModel> { new AddOrderItemViewModel { ProductId = product.Id, Price = 100, Qty = -30 } } };

            var id = await service.AddOrderItemAsync(model);

            var orderItemDB = context.OrderItems.FirstOrDefault();

            Assert.Null(orderItemDB);
        }

        [Fact]
        public async Task DeleteOrderItemsShouldDeleteOrderItems()
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
            var order = new Order { WarehouseId = warehouse.Id };
            context.Orders.Add(order);
            var orderItem = new OrderItem { OrderId = order.Id, ProductId = product.Id, Qty = 3 };
            context.Orders.Add(order);
            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            var mockInventoryService = new Mock<IInventoryService>();
            var mockOrdersService = new Mock<IOrdersService>();
            var service = new OrderItemsService(context, mockInventoryService.Object, mockOrdersService.Object);

            await service.DeleteOrderItemAsync(orderItem.Id);

            var orderItemDB = context.OrderItems.FirstOrDefault();

            Assert.Null(orderItemDB);
        }

        [Fact]
        public async Task AddOrderItemsShouldAddNewItemsToOrderWhenAddingItemsFromProduct()
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
            var order = new Order { WarehouseId = warehouse.Id };
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var mockInventoryService = new Mock<IInventoryService>();
            var mockOrdersService = new Mock<IOrdersService>();
            var service = new OrderItemsService(context, mockInventoryService.Object, mockOrdersService.Object);
            var model = new AddProductToOrderInputModel { OrderId = order.Id, ProductId = product.Id, Qty = 10 };

            var id = await service.AddOrderItemAsync(model);

            var orderItemDB = context.OrderItems.FirstOrDefault();

            Assert.NotNull(orderItemDB);
            Assert.Equal(order.Id, id);
            Assert.True(orderItemDB.OrderId == order.Id);
            Assert.True(orderItemDB.ProductId == product.Id);
        }

        [Fact]
        public async Task AddOrderItemsShouldUpdateQtyIfItemsIsAlreadyAddedWhenAddingItemsFromProduct()
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
            var order = new Order { WarehouseId = warehouse.Id };
            context.Orders.Add(order);
            var orderItem = new OrderItem { OrderId = order.Id, ProductId = product.Id, Qty = 3 };
            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            var mockInventoryService = new Mock<IInventoryService>();
            var mockOrdersService = new Mock<IOrdersService>();
            var service = new OrderItemsService(context, mockInventoryService.Object, mockOrdersService.Object);
            var model = new AddProductToOrderInputModel { OrderId = order.Id, ProductId = product.Id, Qty = -3 };

            var id = await service.AddOrderItemAsync(model);

            var orderItemDB = context.OrderItems.FirstOrDefault();

            Assert.Null(orderItemDB);
        }
    }
}
