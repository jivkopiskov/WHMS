namespace WHMS.Services.Orders
{
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using WHMS.Data;
    using WHMS.Data.Models.Orders;
    using WHMS.Data.Models.Orders.Enum;
    using WHMS.Services.Mapping;
    using WHMS.Services.Products;
    using WHMS.Web.ViewModels.Orders;
    using WHMS.Web.ViewModels.Products;

    public class OrderItemsService : IOrderItemsService
    {
        private readonly WHMSDbContext context;
        private readonly IInventoryService inventoryService;
        private readonly IOrdersService ordersService;

        public OrderItemsService(WHMSDbContext context, IInventoryService inventoryService, IOrdersService ordersService)
        {
            this.context = context;
            this.inventoryService = inventoryService;
            this.ordersService = ordersService;
        }

        public async Task<int> AddOrderItemAsync(AddProductToOrderInputModel input)
        {
            var order = this.context.Orders.FirstOrDefault(x => x.Id == input.OrderId);

            var orderItem = this.context.OrderItems.FirstOrDefault(x => x.ProductId == input.ProductId && x.OrderId == input.OrderId);
            if (orderItem == null)
            {
                orderItem = new OrderItem() { ProductId = input.ProductId, Qty = input.Qty };
                var productPrices = this.context.Products.Where(x => x.Id == orderItem.ProductId).Select(x => new { x.WebsitePrice, x.WholesalePrice }).FirstOrDefault();
                orderItem.Price = order.Channel == Channel.Wholesale ? productPrices.WholesalePrice : productPrices.WebsitePrice;
                order.OrderItems.Add(orderItem);
            }
            else
            {
                orderItem.Qty += input.Qty;
            }

            await this.context.SaveChangesAsync();
            await this.ordersService.RecalculateOrderStatusesAsync(input.OrderId);

            return order.Id;
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
                    var productPrices = this.context.Products.Where(x => x.Id == orderItem.ProductId).Select(x => new { x.WebsitePrice, x.WholesalePrice }).FirstOrDefault();
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
            await this.ordersService.RecalculateOrderStatusesAsync(input.OrderId);

            return order.Id;
        }

        public async Task DeleteOrderItemAsync(int orderItemId)
        {
            var oi = this.context.OrderItems.FirstOrDefault(x => x.Id == orderItemId);
            var orderId = oi.OrderId;
            this.context.OrderItems.Remove(oi);
            await this.context.SaveChangesAsync();
            await this.ordersService.RecalculateOrderStatusesAsync(orderId);
            await this.inventoryService.RecalculateAvailableInventoryAsync(oi.ProductId);
        }
    }
}
