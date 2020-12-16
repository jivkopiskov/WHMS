namespace WHMS.Services.Tests.Products
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using WHMS.Common;
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

        [Fact]
        public async Task AddProductImageShouldAddNewImage()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var product = new Product
            {
                ProductName = "Test Product",
                ShortDescription = "Test Product",
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();
            var mockInventoryService = new Mock<IInventoryService>();
            var service = new ProductsService(context, mockInventoryService.Object);

            var image = new ImageViewModel { ProductId = product.Id, Url = "https://c8.alamy.com/comp/D8RWP0/url-of-web-browser-D8RWP0.jpg" };
            await service.AddProductImageAsync(image);

            Assert.True(context.Images.Count() == 1);
        }

        [Fact]
        public async Task EditProductShouldEditProperties()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var product = new Product
            {
                ProductName = "Test Product",
                ShortDescription = "Test Product",
                WebsitePrice = 23,
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();
            var mockInventoryService = new Mock<IInventoryService>();
            var service = new ProductsService(context, mockInventoryService.Object);

            var editedProduct = new ProductDetailsInputModel { Id = product.Id, ProductName = "Edited", ShortDescription = "Edited", WebsitePrice = 100, };
            await service.EditProductAsync<ProductDetailsViewModel, ProductDetailsInputModel>(editedProduct);

            var dbProduct = context.Products.FirstOrDefault();

            Assert.Equal(editedProduct.ProductName, dbProduct.ProductName);
            Assert.Equal(editedProduct.ShortDescription, dbProduct.ShortDescription);
            Assert.Equal(editedProduct.WebsitePrice, dbProduct.WebsitePrice);
        }

        [Fact]
        public async Task GetAllProductsWithFiltersAndPaging()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            for (int i = 0; i < 100; i++)
            {
                var product = new Product
                {
                    ProductName = "Test Product",
                    ShortDescription = "Test Product",
                    WebsitePrice = 23,
                    BrandId = i % 2,
                };
                context.Products.Add(product);
            }

            await context.SaveChangesAsync();
            var mockInventoryService = new Mock<IInventoryService>();
            var service = new ProductsService(context, mockInventoryService.Object);

            var filter = new ProductFilterInputModel { BrandId = 1 };
            var products = service.GetAllProducts<ProductDetailsViewModel>(filter);

            Assert.Equal(GlobalConstants.PageSize, products.Count());
            foreach (var item in products)
            {
                Assert.Equal(1, item.BrandId);
            }
        }

        [Fact]
        public async Task GetAllProductsWithoutFiltersAndPaging()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            for (int i = 0; i < 100; i++)
            {
                var product = new Product
                {
                    ProductName = "Test Product",
                    ShortDescription = "Test Product",
                    WebsitePrice = 23,
                    BrandId = i % 2,
                };
                context.Products.Add(product);
            }

            await context.SaveChangesAsync();
            var mockInventoryService = new Mock<IInventoryService>();
            var service = new ProductsService(context, mockInventoryService.Object);

            var products = service.GetAllProducts<ProductDetailsViewModel>();

            Assert.Equal(100, products.Count());
        }

        [Fact]
        public async Task GetProductsCountTestWithEmptyFilter()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            for (int i = 0; i < 100; i++)
            {
                var product = new Product
                {
                    ProductName = "Test Product",
                    ShortDescription = "Test Product",
                    WebsitePrice = 23,
                    BrandId = i % 2,
                };
                context.Products.Add(product);
            }

            await context.SaveChangesAsync();
            var mockInventoryService = new Mock<IInventoryService>();
            var service = new ProductsService(context, mockInventoryService.Object);

            var productsCount = service.GetAllProductsCount(new ProductFilterInputModel());

            Assert.Equal(100, productsCount);
        }

        [Fact]
        public async Task GetProductsCountTestWithFilter()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            for (int i = 0; i < 100; i++)
            {
                var product = new Product
                {
                    ProductName = "Test Product",
                    ShortDescription = "Test Product",
                    WebsitePrice = 23,
                    BrandId = i % 2,
                };
                context.Products.Add(product);
            }

            await context.SaveChangesAsync();
            var mockInventoryService = new Mock<IInventoryService>();
            var service = new ProductsService(context, mockInventoryService.Object);

            var productsCount = service.GetAllProductsCount(new ProductFilterInputModel { BrandId = 1 });

            Assert.Equal(50, productsCount);
        }

        [Fact]
        public async Task GetProductsCountTestWithouitFilter()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            for (int i = 0; i < 100; i++)
            {
                var product = new Product
                {
                    ProductName = "Test Product",
                    ShortDescription = "Test Product",
                    WebsitePrice = 23,
                    BrandId = i % 2,
                };
                context.Products.Add(product);
            }

            await context.SaveChangesAsync();
            var mockInventoryService = new Mock<IInventoryService>();
            var service = new ProductsService(context, mockInventoryService.Object);

            var productsCount = service.GetAllProductsCount();

            Assert.Equal(100, productsCount);
        }

        [Fact]
        public async Task GetProductDetails()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var product = new Product
            {
                ProductName = "Test Product",
                ShortDescription = "Test Product",
                WebsitePrice = 23,
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();
            var mockInventoryService = new Mock<IInventoryService>();
            var service = new ProductsService(context, mockInventoryService.Object);

            var productFromService = service.GetProductDetails<ProductDetailsViewModel>(product.Id);

            Assert.Equal(product.ProductName, productFromService.ProductName);
            Assert.Equal(product.ShortDescription, productFromService.ShortDescription);
            Assert.Equal(product.WebsitePrice, productFromService.WebsitePrice);
        }

        [Fact]
        public async Task GetProductImagesTest()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var product = new Product
            {
                ProductName = "Test Product",
                ShortDescription = "Test Product",
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();
            context.Images.Add(new Image { ProductId = product.Id, Url = "https://c8.alamy.com/comp/D8RWP0/url-of-web-browser-D8RWP0.jpg" });
            context.SaveChanges();
            var mockInventoryService = new Mock<IInventoryService>();
            var service = new ProductsService(context, mockInventoryService.Object);

            var images = service.GetProductImages<ImageViewModel>(product.Id);

            Assert.True(images.Count() == 1);
        }

        [Fact]
        public async Task UpdateDefaultImage()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var product = new Product
            {
                ProductName = "Test Product",
                ShortDescription = "Test Product",
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();
            var image = new Image { ProductId = product.Id, Url = "https://c8.alamy.com/comp/D8RWP0/url-of-web-browser-D8RWP0.jpg", IsPrimary = false };
            var image2 = new Image { ProductId = product.Id, Url = "https://c8.alamy.com/comp/D8RWP0/url-of-web-browser-D8RWP0.jpg", IsPrimary = true };
            var image3 = new Image { ProductId = product.Id, Url = "https://c8.alamy.com/comp/D8RWP0/url-of-web-browser-D8RWP0.jpg", IsPrimary = true };
            context.Images.Add(image);
            context.Images.Add(image2);
            context.Images.Add(image3);
            context.SaveChanges();
            var mockInventoryService = new Mock<IInventoryService>();
            var service = new ProductsService(context, mockInventoryService.Object);

            await service.UpdateDefaultImageAsync(image.Id);

            Assert.True(image.IsPrimary);
            Assert.False(image2.IsPrimary);
            Assert.False(image3.IsPrimary);
        }
    }
}
