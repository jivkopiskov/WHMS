namespace WHMS.Services.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using WHMS.Common;
    using WHMS.Data;
    using WHMS.Data.Models.Orders;
    using WHMS.Services.Mapping;
    using WHMS.Web.ViewModels.Orders;
    using WHMS.Web.ViewModels.Orders.Enums;

    public class CustomersService : ICustomersService
    {
        private readonly WHMSDbContext context;
        private IMapper mapper;

        public CustomersService(WHMSDbContext context)
        {
            this.context = context;
            this.mapper = AutoMapperConfig.MapperInstance;
        }

        public Task<int> CreateCustomerAsync()
        {
            throw new NotImplementedException();
        }

        public T GetCustomer<T>(string email)
        {
            var customer = this.context.Customers.Include(x => x.Address).FirstOrDefault(x => x.Email == email);
            if (customer == null)
            {
                return default(T);
            }

            return this.mapper.Map<T>(customer);
        }

        public IEnumerable<T> GetAllCustomers<T>(CustomersFilterInputModel input)
        {
            var customers = this.context.Customers.Where(c => c.IsDeleted == false);
            customers = this.FilterCustomers(input, customers);
            var result = customers
               .Skip((input.Page - 1) * GlobalConstants.PageSize)
               .Take(GlobalConstants.PageSize)
               .To<T>()
               .ToList();

            return result;
        }

        public int CustomersCount()
        {
            return this.context.Customers.Count();
        }

        public Task<int> EditCustomerAsync(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCustomerOrdersAsync(int customerId)
        {
            throw new NotImplementedException();
        }

        private IQueryable<Customer> FilterCustomers(CustomersFilterInputModel input, IQueryable<Customer> customers)
        {
            if (!string.IsNullOrEmpty(input.Email))
            {
                customers = customers.Where(c => c.Email.Contains(input.Email));
            }

            if (!string.IsNullOrEmpty(input.PhoneNumber))
            {
                customers = customers.Where(c => c.PhoneNumber.Contains(input.PhoneNumber));
            }

            if (!string.IsNullOrEmpty(input.ZipCode))
            {
                customers = customers.Where(c => c.Address.ZIP.Contains(input.ZipCode));
            }

            customers = input.Sorting switch
            {
                CustomerSorting.Id => customers.OrderBy(x => x.Id),
                CustomerSorting.IdDesc => customers.OrderByDescending(x => x.Id),
                CustomerSorting.Alphabetically => customers.OrderBy(x => x.Email),
                CustomerSorting.AlphabeticallyDesc => customers.OrderByDescending(x => x.Email),
                CustomerSorting.NumberOfOrders => customers.OrderByDescending(x => x.Orders.Count()),
                _ => customers,
            };
            return customers;
        }
    }
}
