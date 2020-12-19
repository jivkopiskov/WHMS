namespace WHMS.Services.Tests.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using WHMS.Common;
    using WHMS.Data;
    using WHMS.Data.Models;
    using WHMS.Data.Models.Orders;
    using WHMS.Services.Orders;
    using WHMS.Web.ViewModels.Orders;
    using Xunit;

    public class CustomersServiceTests : BaseServiceTest
    {
        [Fact]
        public async Task CreateCustomerShouldCreateNewCustomer()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);

            var service = new CustomersService(context);
            var model = new CustomerViewModel { Email = "Test@gmail.com", FirstName = "Test", LastName = "Testov", PhoneNumber = "123451234" };

            await service.CreateOrUpdateCustomerAsync(model);

            var dbCustomer = context.Customers.FirstOrDefault();

            Assert.NotNull(dbCustomer);
            Assert.Equal("Test@gmail.com", dbCustomer.Email);
            Assert.Equal("Test", dbCustomer.FirstName);
            Assert.Equal("Testov", dbCustomer.LastName);
            Assert.Equal("123451234", dbCustomer.PhoneNumber);
        }

        [Fact]
        public async Task CreateCustomerShouldUpdateExistingCustomer()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            context.Customers.Add(new Customer { Email = "Test@gmail.com", FirstName = "Pesho", LastName = "Peshov", PhoneNumber = "000000" });
            await context.SaveChangesAsync();

            var service = new CustomersService(context);
            var model = new CustomerViewModel { Email = "Test@gmail.com", FirstName = "Test", LastName = "Testov", PhoneNumber = "123451234" };

            await service.CreateOrUpdateCustomerAsync(model);

            var dbCustomer = context.Customers.FirstOrDefault();

            Assert.NotNull(dbCustomer);
            Assert.Equal("Test@gmail.com", dbCustomer.Email);
            Assert.Equal("Test", dbCustomer.FirstName);
            Assert.Equal("Testov", dbCustomer.LastName);
            Assert.Equal("123451234", dbCustomer.PhoneNumber);
        }

        [Fact]
        public async Task GetCustomerShouldReturnCustomerIfExists()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            context.Customers.Add(new Customer { Email = "test@gmail.com", FirstName = "Pesho", LastName = "Peshov", PhoneNumber = "000000", Address = new Address { } });
            await context.SaveChangesAsync();

            var service = new CustomersService(context);

            var customerFromService = service.GetCustomer<CustomerViewModel>("test@gmail.com");

            var dbCustomer = context.Customers.FirstOrDefault();

            Assert.NotNull(dbCustomer);
            Assert.Equal(customerFromService.Email, dbCustomer.Email);
            Assert.Equal(customerFromService.FirstName, dbCustomer.FirstName);
            Assert.Equal(customerFromService.LastName, dbCustomer.LastName);
            Assert.Equal(customerFromService.PhoneNumber, dbCustomer.PhoneNumber);
        }

        [Fact]
        public async Task GetCustomersCountShouldBeCorrect()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            context.Customers.Add(new Customer { Email = "test@gmail.com", FirstName = "Pesho", LastName = "Peshov", PhoneNumber = "000000", Address = new Address { } });
            await context.SaveChangesAsync();

            var service = new CustomersService(context);

            var count = service.CustomersCount();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task GetAllCustomersWithPagingAndFiltering()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            for (int i = 0; i < 12; i++)
            {
                context.Customers.Add(new Customer { Email = i + "@gmail.com", FirstName = "Pesho", LastName = "Peshov", PhoneNumber = "000000", Address = new Address { } });
            }
            await context.SaveChangesAsync();

            var service = new CustomersService(context);

            var filters = new CustomersFilterInputModel { Page = 1, Email = "1" };
            var customers = service.GetAllCustomers<CustomerViewModel>(filters);

            Assert.Equal(3, customers.Count());
        }


        [Fact]
        public async Task GetAllCustomersWithPagingShouldReturnOnly1Page()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            for (int i = 0; i < 100; i++)
            {
                context.Customers.Add(new Customer { Email = i + "@gmail.com", FirstName = "Pesho", LastName = "Peshov", PhoneNumber = "000000", Address = new Address { } });
            }
            await context.SaveChangesAsync();

            var service = new CustomersService(context);

            var filters = new CustomersFilterInputModel { Page = 1};
            var customers = service.GetAllCustomers<CustomerViewModel>(filters);

            Assert.Equal(GlobalConstants.PageSize, customers.Count());
        }
    }
}
