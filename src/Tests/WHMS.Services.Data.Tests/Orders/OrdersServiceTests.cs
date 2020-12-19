namespace WHMS.Services.Tests.Orders
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using WHMS.Data;
    using WHMS.Data.Models;
    using WHMS.Data.Models.Orders;
    using WHMS.Data.Models.Orders.Enum;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Orders;
    using WHMS.Services.Products;
    using WHMS.Web.ViewModels.Orders;
    using Xunit;

    public class OrdersServiceTests : BaseServiceTest
    {
        [Fact]
        public async Task RecalculateOrderReservesCallsInventoryServiceProperly()
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
            var mockCustomersService = new Mock<ICustomersService>();

            var service = new OrdersService(context, mockInventoryService.Object, mockCustomersService.Object);

            await service.RecalculateOrderReservesAsync(order.Id);

            mockInventoryService.Verify(x => x.RecalculateAvailableInventoryAsync(It.IsAny<int>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task CancelOrderShouldSetOrderStatusToCancelled()
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
            var mockCustomersService = new Mock<ICustomersService>();

            var service = new OrdersService(context, mockInventoryService.Object, mockCustomersService.Object);

            await service.CancelOrderAsync(order.Id);

            Assert.True(order.OrderStatus == OrderStatus.Cancelled);
            mockInventoryService.Verify(x => x.RecalculateAvailableInventoryAsync(It.IsAny<int>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task SetInProcessOrderShouldSetOrderStatusToProcessing()
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
            var mockCustomersService = new Mock<ICustomersService>();

            var service = new OrdersService(context, mockInventoryService.Object, mockCustomersService.Object);

            await service.SetInProcessAsync(order.Id);

            Assert.True(order.OrderStatus == OrderStatus.Processing);
            mockInventoryService.Verify(x => x.RecalculateAvailableInventoryAsync(It.IsAny<int>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task AddPaymentShouldAddPaymentAndUpdateStatus()
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
            var order = new Order { WarehouseId = warehouse.Id, GrandTotal = 100 };
            context.Orders.Add(order);
            var orderItem = new OrderItem { OrderId = order.Id, ProductId = product.Id, Qty = 3 };
            context.Orders.Add(order);
            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            var mockInventoryService = new Mock<IInventoryService>();
            var mockCustomersService = new Mock<ICustomersService>();

            var service = new OrdersService(context, mockInventoryService.Object, mockCustomersService.Object);
            var payment = new PaymentInputModel { OrderId = order.Id, Amount = 100 };
            await service.AddPaymentAsync(payment);
            var paymentDb = context.Payments.FirstOrDefault();

            Assert.NotNull(paymentDb);
            Assert.True(order.PaymentStatus == PaymentStatus.FullyCharged);
            Assert.Equal(100, paymentDb.Amount);
        }

        [Fact]
        public async Task AddPartialPaymentShouldAddPaymentAndUpdateStatus()
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
            var order = new Order { WarehouseId = warehouse.Id, GrandTotal = 100 };
            context.Orders.Add(order);
            var orderItem = new OrderItem { OrderId = order.Id, ProductId = product.Id, Qty = 3 };
            context.Orders.Add(order);
            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            var mockInventoryService = new Mock<IInventoryService>();
            var mockCustomersService = new Mock<ICustomersService>();

            var service = new OrdersService(context, mockInventoryService.Object, mockCustomersService.Object);
            var payment = new PaymentInputModel { OrderId = order.Id, Amount = 50 };
            await service.AddPaymentAsync(payment);
            var paymentDb = context.Payments.FirstOrDefault();

            Assert.NotNull(paymentDb);
            Assert.True(order.PaymentStatus == PaymentStatus.PartiallyPaid);
            Assert.Equal(50, paymentDb.Amount);
        }

        [Fact]
        public async Task DeletePaymentShouldAddPaymentAndUpdateStatus()
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
            var order = new Order { WarehouseId = warehouse.Id, GrandTotal = 100 };
            context.Orders.Add(order);
            var orderItem = new OrderItem { OrderId = order.Id, ProductId = product.Id, Qty = 3 };
            context.Orders.Add(order);
            context.OrderItems.Add(orderItem);
            var payment = new Payment { Amount = 100, OrderId = order.Id, };
            context.Payments.Add(payment);
            await context.SaveChangesAsync();

            var mockInventoryService = new Mock<IInventoryService>();
            var mockCustomersService = new Mock<ICustomersService>();

            var service = new OrdersService(context, mockInventoryService.Object, mockCustomersService.Object);
            await service.DeletePaymentAsync(payment.Id);
            var paymentDb = context.Payments.FirstOrDefault();

            Assert.Null(paymentDb);
            Assert.True(order.PaymentStatus == PaymentStatus.NoPayment);
        }

        [Fact]
        public async Task AddOrderShouldCreateNewOrder()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var warehouse = new Warehouse
            {
                Address = new Address { },
                Name = "Test",
            };

            context.Warehouses.Add(warehouse);

            await context.SaveChangesAsync();

            var mockInventoryService = new Mock<IInventoryService>();
            var mockCustomersService = new Mock<ICustomersService>();

            var service = new OrdersService(context, mockInventoryService.Object, mockCustomersService.Object);

            var model = new AddOrderInputModel { Channel = 0, Customer = new CustomerViewModel { }, SourceOrderId = "test", };
            await service.AddOrderAsync(model);

            var dbOrder = context.Orders.FirstOrDefault();

            Assert.NotNull(dbOrder);
        }

        [Fact]
        public async Task DeleteOrderShouldDeleteOrder()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var warehouse = new Warehouse
            {
                Address = new Address { },
                Name = "Test",
            };

            context.Warehouses.Add(warehouse);

            var order = new Order { WarehouseId = warehouse.Id };
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var mockInventoryService = new Mock<IInventoryService>();
            var mockCustomersService = new Mock<ICustomersService>();

            var service = new OrdersService(context, mockInventoryService.Object, mockCustomersService.Object);

            await service.DeleteOrderAsync(order.Id);

            var dbOrder = context.Orders.FirstOrDefault();

            Assert.Null(dbOrder);
        }
    }
}
