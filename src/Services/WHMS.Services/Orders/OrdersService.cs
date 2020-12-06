namespace WHMS.Services.Orders
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using WHMS.Common;
    using WHMS.Data;
    using WHMS.Data.Models.Orders;
    using WHMS.Data.Models.Orders.Enum;
    using WHMS.Services.Mapping;
    using WHMS.Services.Products;
    using WHMS.Web.ViewModels;
    using WHMS.Web.ViewModels.Orders;
    using WHMS.Web.ViewModels.Orders.Enums;
    using WHMS.Web.ViewModels.Products;

    public class OrdersService : IOrdersService
    {
        private readonly WHMSDbContext context;
        private readonly IInventoryService inventoryService;
        private readonly ICustomersService customersService;
        private readonly IMapper mapper;

        public OrdersService(WHMSDbContext context, IInventoryService inventoryService, ICustomersService customersService)
        {
            this.context = context;
            this.inventoryService = inventoryService;
            this.customersService = customersService;
            this.mapper = AutoMapperConfig.MapperInstance;
        }

        public async Task RecalculateOrderReservesAsync(int orderId)
        {
            var orderItems = this.context.OrderItems.Where(oi => oi.OrderId == orderId).ToList();
            foreach (var item in orderItems)
            {
                await this.inventoryService.RecalculateAvailableInventoryAsync(item.ProductId);
            }
        }

        public async Task CancelOrderAsync(int orderId)
        {
            var order = this.context.Orders.FirstOrDefault(o => o.Id == orderId);
            order.OrderStatus = OrderStatus.Cancelled;
            await this.context.SaveChangesAsync();
            await this.RecalculateOrderReservesAsync(orderId);
        }

        public async Task SetInProcessAsync(int orderId)
        {
            var order = this.context.Orders.FirstOrDefault(o => o.Id == orderId);
            order.OrderStatus = OrderStatus.Processing;
            await this.context.SaveChangesAsync();
            await this.RecalculateOrderReservesAsync(orderId);
        }

        public async Task AddPaymentAsync<T>(T input)
        {
            var payment = this.mapper.Map<Payment>(input);
            await this.context.Payments.AddAsync(payment);
            await this.context.SaveChangesAsync();

            await this.RecalculatePaymentStatusAsync(payment.OrderId);
        }

        public IEnumerable<T> GetAllPayments<T>(int orderId)
        {
            return this.context.Payments.Where(x => x.OrderId == orderId && x.IsDeleted == false).To<T>().ToList();
        }

        public async Task RecalculatePaymentStatusAsync(int orderId)
        {
            var order = this.context.Orders.FirstOrDefault(o => o.Id == orderId);
            var totalPaid = this.context.Payments.Where(p => p.OrderId == orderId && p.IsDeleted == false).Sum(p => p.Amount);

            if (totalPaid == 0)
            {
                order.PaymentStatus = PaymentStatus.NoPayment;
            }
            else if (totalPaid >= order.GrandTotal)
            {
                order.PaymentStatus = PaymentStatus.FullyCharged;
            }
            else
            {
                order.PaymentStatus = PaymentStatus.PartiallyPaid;
            }

            await this.context.SaveChangesAsync();
        }

        public async Task DeletePaymentAsync(int paymentId)
        {
            var payment = this.context.Payments.FirstOrDefault(p => p.Id == paymentId);
            this.context.Remove(payment);
            await this.context.SaveChangesAsync();
            await this.RecalculatePaymentStatusAsync(payment.OrderId);
        }

        public async Task<int> AddOrderAsync(AddOrderInputModel input)
        {
            var customer = this.customersService.GetCustomer<Customer>(input.Customer.Email);
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

        public async Task DeleteOrderAsync(int orderId)
        {
            this.context.Orders.Remove(this.context.Orders.Find(orderId));
            await this.context.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllOrders<T>(OrdersFilterInputModel input)
        {
            var orders = this.context.Orders.Where(x => x.IsDeleted == false);
            var filteredList = this.FilterOrders(input, orders);
            filteredList = filteredList.OrderByDescending(x => x.Id);

            return filteredList
                .Skip((input.Page - 1) * GlobalConstants.PageSize)
                .Take(GlobalConstants.PageSize)
                .To<T>()
                .ToList();
        }

        public int GetAllOrdersCount()
        {
            return this.context.Orders.Where(x => !x.IsDeleted).Count();
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

        public async Task RecalculateOrderStatusesAsync(int orderId)
        {
            await this.RecalculateOrderTotal(orderId);
            await this.RecalculatePaymentStatusAsync(orderId);
            await this.RecalculateOrderReservesAsync(orderId);
        }

        private IQueryable<Order> FilterOrders(OrdersFilterInputModel input, IQueryable<Order> orders)
        {
            if (input.PaymentStatus != null)
            {
                orders = orders.Where(x => x.PaymentStatus == input.PaymentStatus);
            }

            if (input.ShippingStatus != null)
            {
                orders = orders.Where(x => x.ShippingStatus == input.ShippingStatus);
            }

            if (input.WarehouseId != null && input.WarehouseId != 0)
            {
                orders = orders.Where(x => x.WarehouseId == input.WarehouseId);
            }

            if (input.OrderStatus != null)
            {
                orders = orders.Where(x => x.OrderStatus == input.OrderStatus);
            }

            if (!string.IsNullOrEmpty(input.SKU))
            {
                orders = orders.Where(x => x.OrderItems.Any(oi => oi.ProductId.ToString() == input.SKU || oi.Product.SKU == input.SKU));
            }

            if (!string.IsNullOrEmpty(input.CustomerEmail))
            {
                orders = orders.Where(x => x.Customer.Email == input.CustomerEmail);
            }

            return orders;
        }
    }
}
