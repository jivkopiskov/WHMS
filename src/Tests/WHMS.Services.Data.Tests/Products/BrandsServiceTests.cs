namespace WHMS.Services.Tests.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using WHMS.Common;
    using WHMS.Data;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;
    using WHMS.Services.Products;
    using WHMS.Web.ViewModels;
    using WHMS.Web.ViewModels.Products;
    using Xunit;

    public class BrandsServiceTests : BaseServiceTest
    {
        [Fact]
        public async Task CreateBrandShouldCreateNewBrand()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: "TestDB").Options;
            using var context = new WHMSDbContext(options);
            var service = new BrandsService(context);

            var brandId = await service.CreateBrandAsync("TestBrand");

            var brandsCount = service.GetAllBrandsCount();
            var expectedCount = 1;

            Assert.Equal(expectedCount, brandsCount);
            Assert.Equal(1, brandId);
        }

        [Fact]
        public async Task DeleteBrandShouldDeleteExistingBrand()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            await context.Brands.AddAsync(new Brand { Id = 10 });
            var service = new BrandsService(context);

            var success = await service.DeleteBrandAsync(10);

            Assert.True(success);
        }

        [Fact]
        public async Task DeleteBrandShouldReturnFalseBrandDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            await context.Brands.AddAsync(new Brand { Name = "Test" });
            await context.SaveChangesAsync();
            var service = new BrandsService(context);

            var success = await service.DeleteBrandAsync(20);

            Assert.False(success);
        }

        [Fact]
        public async Task GetAllBrandsShouldReturnAllBrands()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            for (int i = 0; i < 100; i++)
            {
                await context.Brands.AddAsync(new Brand { Name = i.ToString() });
            }

            await context.SaveChangesAsync();
            var service = new BrandsService(context);

            var brands = service.GetAllBrands<BrandViewModel>();
            var brandsCount = brands.ToList().Count();
            var exepcetedCount = context.Brands.Count();

            Assert.Equal(exepcetedCount, brandsCount);
        }

        [Fact]
        public async Task GetAllBrandsShouldReturnMaxPageSize()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            for (int i = 0; i < 100; i++)
            {
                await context.Brands.AddAsync(new Brand { Name = i.ToString() });
            }

            await context.SaveChangesAsync();
            var service = new BrandsService(context);

            var brands = service.GetAllBrands<BrandViewModel>(1);
            var brandsCount = brands.ToList().Count();
            var exepcetedCount = GlobalConstants.PageSize;

            Assert.Equal(exepcetedCount, brandsCount);
        }
    }
}
