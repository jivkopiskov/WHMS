namespace WHMS.Services.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using WHMS.Data;
    using WHMS.Data.Models.Orders;
    using WHMS.Data.Models.Orders.Enum;
    using WHMS.Services.Mapping;
    using WHMS.Services.Products;
    using WHMS.Web.ViewModels.Orders;

    public class OrdersService : IOrdersService
    {
        private readonly WHMSDbContext context;
        private readonly IProductsService productsService;
        private IMapper mapper;

        public OrdersService(WHMSDbContext context, IProductsService productsService)
        {
            this.context = context;
            this.productsService = productsService;
            this.mapper = AutoMapperConfig.MapperInstance;
        }

        public Task<int> AddCarrierAsync(string carrierName)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddOrderItemAsync(AddOrderItemsInputModel input)
        {
            var order = this.context.Orders.FirstOrDefault(x => x.Id == input.OrderId);
            foreach (var item in input.OrderItems)
            {
                var orderItem = this.context.OrderItems.FirstOrDefault(x => x.ProductId == item.ProductId && x.OrderId == input.OrderId);
                if (orderItem == null)
                {
                    orderItem = new OrderItem() { ProductId = item.ProductId, Qty = item.Qty };
                    var productPrices = this.context.Products.Where(x => x.Id == orderItem.ProductId).Select(x => new { WebsitePrice = x.WebsitePrice, WholesalePrice = x.WholesalePrice }).FirstOrDefault();
                    orderItem.Price = order.Channel == Channel.Wholesale ? productPrices.WholesalePrice : productPrices.WebsitePrice;
                    order.OrderItems.Add(orderItem);
                }
                else
                {
                    orderItem.Qty += item.Qty;
                    if (orderItem.Qty == 0)
                    {
                        this.context.OrderItems.Remove(orderItem);
                    }
                }
            }

            await this.context.SaveChangesAsync();
            await this.RecalculateOrderTotal(input.OrderId);
            foreach (var item in input.OrderItems)
            {
                await this.productsService.RecalculateAvailableInventory(item.ProductId);
            }

            return order.Id;
        }

        public Task<int> AddPaymentAsync(int orderId, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddShippingMethodAsync(Carrier carrier, string shippingMethod)
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateCustomerAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<int> CreateOrderAsync(AddOrderInputModel input)
        {
            var customer = this.GetCustomer<Customer>(input.Customer.Email);
            customer = this.mapper.Map<CustomerViewModel, Customer>(input.Customer, customer);
            var order = this.mapper.Map<Order>(input);
            order.Customer = customer;
            order.PaymentStatus = PaymentStatus.NoPayment;
            order.ShippingStatus = ShippingStatus.Unshipped;
            order.OrderStatus = OrderStatus.Processing;
            if (order.WarehouseId == 0)
            {
                order.WarehouseId = this.context.Warehouses.Select(x => x.Id).FirstOrDefault();
            }

            this.context.Orders.Add(order);

            await this.context.SaveChangesAsync();

            return order.Id;
        }

        public Task<int> DeleteOrderItemAsync(int orderItemId)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditCustomerAsync(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditOrderAsync(int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditOrderItemAsync(int orderItemId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAllOrders<T>(OrdersFilterInputModel input)
        {
            return this.context.Orders.To<T>().ToList();
        }

        public Task<int> GetCustomerOrdersAsync(int customerId)
        {
            throw new NotImplementedException();
        }

        public T GetOrderDetails<T>(int orderId)
        {
            return this.context.Orders.Where(x => x.Id == orderId).To<T>().FirstOrDefault();
        }

        public async Task RecalculateOrderTotal(int orderId)
        {
            var order = this.context.Orders.Include(x => x.OrderItems).FirstOrDefault(x => x.Id == orderId);
            order.GrandTotal = order.OrderItems.Sum(x => x.Qty * x.Price);
            await this.context.SaveChangesAsync();
        }

        public Task<int> ShipOrderAsync(int orderId, string shippingMethod, string trackingNumber)
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
    }
}
