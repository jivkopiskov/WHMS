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
        private readonly IInventoryService inventoryService;
        private IMapper mapper;

        public OrdersService(WHMSDbContext context, IInventoryService inventoryService)
        {
            this.context = context;
            this.inventoryService = inventoryService;
            this.mapper = AutoMapperConfig.MapperInstance;
        }

        public async Task<int> AddCarrierAsync(string carrierName)
        {
            var carrier = this.context.Carriers.FirstOrDefault(c => c.Name.ToLower() == carrierName.ToLower());
            if (carrier != null)
            {
                return carrier.Id;
            }

            carrier = new Carrier { Name = carrierName };
            await this.context.AddAsync(carrier);
            await this.context.SaveChangesAsync();

            return carrier.Id;
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
            await this.RecalculatePaymentStatusAsync(input.OrderId);

            await this.RecalculateOrderReservesAsync(input.OrderId);

            return order.Id;
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

        public async Task<int> AddShippingMethodAsync(int carrierId, string shippingMethod)
        {
            var newMethod = this.context.ShippingMethods.Where(sm => sm.CarrierId == carrierId).FirstOrDefault(sm => sm.Name.ToLower() == shippingMethod.ToLower());
            if (newMethod != null)
            {
                return newMethod.Id;
            }

            newMethod = new ShippingMethod { Name = shippingMethod, CarrierId = carrierId };
            await this.context.AddAsync(newMethod);
            await this.context.SaveChangesAsync();

            return newMethod.Id;
        }

        public Task<int> CreateCustomerAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddOrderAsync(AddOrderInputModel input)
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

        public async Task ShipOrderAsync(ShipOrderInputModel input)
        {
            var order = this.context.Orders.FirstOrDefault(x => x.Id == input.OrderId);
            if (order.ShippingStatus == ShippingStatus.Shipped)
            {
                return;
            }

            order.ShippingMethod = this.context.ShippingMethods.FirstOrDefault(x => x.Id == input.ShippingMethod.Id);
            order.ShippingStatus = ShippingStatus.Shipped;
            order.TrackingNumber = input.TrackingNumber;
            order.OrderStatus = OrderStatus.Completed;
            await this.context.SaveChangesAsync();

            await this.inventoryService.RecalculateInventoryAfterShippingAsync(order.Id, order.WarehouseId);
        }

        public async Task UnshipOrderAsync(int orderId)
        {
            var order = this.context.Orders.FirstOrDefault(x => x.Id == orderId);
            if (order.ShippingStatus == ShippingStatus.Unshipped)
            {
                return;
            }

            order.ShippingStatus = ShippingStatus.Unshipped;
            order.OrderStatus = OrderStatus.Processing;
            await this.context.SaveChangesAsync();

            await this.inventoryService.RecalculateInventoryAfterUnshippingAsync(order.Id, order.WarehouseId);
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

        public IEnumerable<T> GetAllCarriers<T>()
        {
            return this.context.Carriers.To<T>().ToList();
        }

        public IEnumerable<T> GetAllServicesForCarrier<T>(int carrierId)
        {
            return this.context.ShippingMethods.Where(x => x.CarrierId == carrierId).To<T>().ToList();
        }
    }
}
