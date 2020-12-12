namespace WHMS.Services.Tests.Products
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using WHMS.Common;
    using WHMS.Data;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Products;
    using WHMS.Web.ViewModels.Products;
    using Xunit;

    public class ManufacturersServiceTests : BaseServiceTest
    {
        [Fact]
        public async Task CreateManufacturerShouldCreateNewManufacturer()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var service = new ManufacturersService(context);

            var manufacturerId = await service.CreateManufacturerAsync("TestManufacturer");

            var manufacturersCount = service.GetAllManufacturersCount();
            var expectedCount = 1;

            Assert.Equal(expectedCount, manufacturersCount);
            Assert.Equal(1, manufacturerId);
        }

        [Fact]
        public async Task GetAllManufacturersShouldReturnAllManufacturers()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            for (int i = 0; i < 100; i++)
            {
                await context.Manufacturers.AddAsync(new Manufacturer { Name = i.ToString() });
            }

            await context.SaveChangesAsync();
            var service = new ManufacturersService(context);

            var manufacturers = service.GetAllManufacturers<ManufacturerViewModel>();
            var manufacturersCount = manufacturers.ToList().Count();
            var exepcetedCount = context.Manufacturers.Count();

            Assert.Equal(exepcetedCount, manufacturersCount);
        }

        [Fact]
        public async Task GetAllManufacturersShouldReturnMaxPageSize()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            for (int i = 0; i < 100; i++)
            {
                await context.Manufacturers.AddAsync(new Manufacturer { Name = i.ToString() });
            }

            await context.SaveChangesAsync();
            var service = new ManufacturersService(context);

            var manufacturers = service.GetAllManufacturers<ManufacturerViewModel>(1);
            var manufacturersCount = manufacturers.ToList().Count();
            var exepcetedCount = GlobalConstants.PageSize;

            Assert.Equal(exepcetedCount, manufacturersCount);
        }
    }
}
