namespace WHMS.Services.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;
    using WHMS.Data;
    using WHMS.Data.Models.Orders;
    using WHMS.Data.Models.Orders.Enum;
    using WHMS.Services.Mapping;
    using WHMS.Services.Products;
    using WHMS.Web.ViewModels.Orders;

    public class ShippingService : IShippingService
    {
        private readonly WHMSDbContext context;
        private readonly IInventoryService inventoryService;

        public ShippingService(WHMSDbContext context, IInventoryService inventoryService)
        {
            this.context = context;
            this.inventoryService = inventoryService;
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

        public async Task DeleteShippingMethodAsync(int id)
        {
            var method = this.context.ShippingMethods.FirstOrDefault(x => x.Id == id);
            this.context.Remove(method);
            await this.context.SaveChangesAsync();
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
