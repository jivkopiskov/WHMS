namespace WHMS.Services.Tests.Products
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using WHMS.Data;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Products;
    using WHMS.Web.ViewModels.Products;
    using Xunit;

    public class ProductsServiceTests : BaseServiceTest
    {
        [Fact]
        public async Task CreateProductShouldCreateNewProduct()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var product = new AddProductInputModel
            {
                ProductName = "Test Product",
                ShortDescription = "Test Product",
            };

            await context.SaveChangesAsync();
            var mockInventoryService = new Mock<IInventoryService>();
            var service = new ProductsService(context, mockInventoryService.Object);

            var productId = await service.CreateProductAsync(product);
            var dbProduct = context.Products.FirstOrDefault();

            Assert.Equal(dbProduct.Id, productId);
            Assert.True(context.Products.Count() == 1);
        }

        [Theory]
        [InlineData("Test", false)]
        [InlineData("TestAvailable", true)]
        public async Task IsSkuAvailableShouldReturnCorrectAnswer(string sku, bool expected)
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var product = new Product
            {
                SKU = "Test",
                ProductName = "Test Product",
                ShortDescription = "Test Product",
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();
            var mockInventoryService = new Mock<IInventoryService>();
            var service = new ProductsService(context, mockInventoryService.Object);

            var result = service.IsSkuAvailable(sku);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1234, false)]
        [InlineData(123, true)]
        public async Task IsValidProduShouldReturnCorrectAnswer(int id, bool expected)
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var product = new Product
            {
                Id = 123,
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();
            var mockInventoryService = new Mock<IInventoryService>();
            var service = new ProductsService(context, mockInventoryService.Object);

            var result = service.IsValidProductId(id);
            Assert.Equal(expected, result);
        }
    }
}
