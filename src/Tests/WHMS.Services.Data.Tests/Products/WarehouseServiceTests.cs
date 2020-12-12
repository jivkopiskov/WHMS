namespace WHMS.Services.Tests.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using WHMS.Data;
    using WHMS.Data.Models;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;
    using WHMS.Services.Products;
    using WHMS.Web.ViewModels;
    using WHMS.Web.ViewModels.Products;
    using Xunit;

    public class WarehouseServiceTests : BaseServiceTest
    {
        [Fact]
        public async Task CreateWarehouseShouldCreateNewWarehouse()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var service = new WarehouseService(context);

            var warehouse = new WarehouseViewModel
            {
                Address = new AddressViewModel
                {
                    City = "Test",
                    StreetAddress = "Test",
                    Zip = "test",
                    Country = "Test",
                },
                Name = "Test",
                IsSellable = true,
            };

            await service.CreateWarehouseAsync(warehouse);

            var warehouseCount = context.Warehouses.Count();
            var expectedCount = 1;

            Assert.Equal(expectedCount, warehouseCount);
        }

        [Fact]
        public async Task GetAllWarehousesShouldReturnAllWarehouses()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            for (int i = 0; i < 5; i++)
            {
                await context.Warehouses.AddAsync(new Warehouse
                {
                    Address = new Address
                    {
                        City = "Test",
                        StreetAddress = "Test",
                        ZIP = "test",
                        Country = "Test",
                    },
                    Name = "Test",
                });
            }

            await context.SaveChangesAsync();
            var service = new WarehouseService(context);

            var warehouses = service.GetAllWarehouses<WarehouseViewModel>();
            var warehousessCount = warehouses.ToList().Count();
            var exepcetedCount = context.Warehouses.Count();

            Assert.Equal(exepcetedCount, warehousessCount);
        }

        [Fact]
        public async Task GetProductWarehousesShouldReturnCorrectInfoWhenProductWarehouseExists()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var warehouse = new Warehouse
            {
                Address = new Address
                {
                    City = "Test",
                    StreetAddress = "Test",
                    ZIP = "test",
                    Country = "Test",
                },
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

            var service = new WarehouseService(context);
            var serviceProductWarehouse = service.GetProductWarehouseInfo(product.Id).FirstOrDefault();

            Assert.Equal(productWarehouse.ProductId, serviceProductWarehouse.ProductId);
            Assert.Equal(productWarehouse.Warehouse.Name, serviceProductWarehouse.WarehouseName);
            Assert.Equal(productWarehouse.TotalPhysicalQuanitiy, serviceProductWarehouse.TotalPhysicalQuanitity);
            Assert.Equal(productWarehouse.TotalPhysicalQuanitiy, serviceProductWarehouse.AggregateQuantity);
            Assert.Equal(productWarehouse.TotalPhysicalQuanitiy, serviceProductWarehouse.ReservedQuantity);
        }

        [Fact]
        public async Task GetProductWarehousesShouldReturnCorrectInfoWhenProductWarehouseDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var warehouse = new Warehouse
            {
                Address = new Address
                {
                    City = "Test",
                    StreetAddress = "Test",
                    ZIP = "test",
                    Country = "Test",
                },
                Name = "Test",
            };
            var product = new Product
            {
                ProductName = "Test Product",
            };

            context.Warehouses.Add(warehouse);
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var service = new WarehouseService(context);
            var serviceProductWarehouse = service.GetProductWarehouseInfo(product.Id).FirstOrDefault();

            Assert.Equal(product.Id, serviceProductWarehouse.ProductId);
            Assert.Equal(warehouse.Name, serviceProductWarehouse.WarehouseName);
            Assert.Equal(0, serviceProductWarehouse.TotalPhysicalQuanitity);
            Assert.Equal(0, serviceProductWarehouse.AggregateQuantity);
            Assert.Equal(0, serviceProductWarehouse.ReservedQuantity);
        }
    }
}
