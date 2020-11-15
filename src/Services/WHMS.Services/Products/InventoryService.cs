namespace WHMS.Services.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;
    using WHMS.Data;
    using WHMS.Data.Models.Orders.Enum;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;
    using WHMS.Web.ViewModels.Products;

    public class InventoryService : IInventoryService
    {
        private WHMSDbContext context;
        private IMapper mapper;

        public InventoryService(WHMSDbContext context)
        {
            this.context = context;
            this.mapper = AutoMapperConfig.MapperInstance;
        }

        public int GetProductAvailableInventory(int productId)
        {
            return this.context.ProductWarehouses.Where(x => x.ProductId == productId).Sum(x => x.AggregateQuantity);
        }

        public async Task RecalculateAvailableInventoryAsync(int productId)
        {
            var warehouses = this.context.Warehouses.ToList();
            foreach (var wh in warehouses)
            {
                await this.RecalculateReservedInventoryAsync(productId, wh.Id);
            }

            var productWarehouses = this.context.ProductWarehouses.Where(x => x.ProductId == productId).ToList();
            foreach (var pw in productWarehouses)
            {
                pw.AggregateQuantity = pw.TotalPhysicalQuanitiy - pw.ReservedQuantity;
            }

            await this.context.SaveChangesAsync();
        }

        public async Task RecalculateReservedInventoryAsync(int productId, int warehouseId)
        {
            var productWarehouse = await this.GetOrCreateProductWarehouseAsync(productId, warehouseId);
            productWarehouse.ReservedQuantity = this.context.OrderItems
                .Where(x => x.ProductId == productId &&
                            x.Order.ShippingStatus == ShippingStatus.Unshipped &&
                            x.Order.OrderStatus == OrderStatus.Processing &&
                            x.Order.WarehouseId == warehouseId)
                .Sum(x => x.Qty);

            this.context.ProductWarehouses.Update(productWarehouse);
            await this.context.SaveChangesAsync();
        }

        public async Task<bool> AdjustInventory(ProductAdjustmentInputModel input)
        {
            var productWarehouse = this.context.ProductWarehouses
                .FirstOrDefault(
                x => x.ProductId == input.ProductId
                && x.WarehouseId == input.WarehouseId);
            if (productWarehouse == null)
            {
                productWarehouse = new ProductWarehouse() { ProductId = input.ProductId, WarehouseId = input.WarehouseId };
                this.context.ProductWarehouses.Add(productWarehouse);
            }

            productWarehouse.TotalPhysicalQuanitiy += input.Qty;
            if (productWarehouse.TotalPhysicalQuanitiy < 0)
            {
                return false;
            }

            await this.context.SaveChangesAsync();
            await this.RecalculateAvailableInventoryAsync(input.ProductId);

            return true;
        }

        public async Task RecalculateInventoryAfterShippingAsync(int orderId, int warehouseId)
        {
            var orderItems = this.context.OrderItems.Where(oi => oi.OrderId == orderId).ToList();

            foreach (var item in orderItems)
            {
                var productWarehouse = await this.GetOrCreateProductWarehouseAsync(item.ProductId, warehouseId);
                productWarehouse.TotalPhysicalQuanitiy -= item.Qty;
                await this.context.SaveChangesAsync();
                await this.RecalculateAvailableInventoryAsync(item.ProductId);
            }
        }

        public async Task RecalculateInventoryAfterUnshippingAsync(int orderId, int warehouseId)
        {
            var orderItems = this.context.OrderItems.Where(oi => oi.OrderId == orderId).ToList();

            foreach (var item in orderItems)
            {
                var productWarehouse = await this.GetOrCreateProductWarehouseAsync(item.ProductId, warehouseId);
                productWarehouse.TotalPhysicalQuanitiy += item.Qty;
                await this.context.SaveChangesAsync();
                await this.RecalculateAvailableInventoryAsync(item.ProductId);
            }
        }

        private async Task<ProductWarehouse> GetOrCreateProductWarehouseAsync(int productId, int warehouseId)
        {
            var productWarehouse = this.context.ProductWarehouses
                .Where(x => x.ProductId == productId &&
                x.WarehouseId == warehouseId)
                .FirstOrDefault();

            if (productWarehouse == null)
            {
                productWarehouse = new ProductWarehouse() { ProductId = productId, WarehouseId = warehouseId };
                this.context.Add(productWarehouse);
                await this.context.SaveChangesAsync();
            }

            return productWarehouse;
        }
    }
}
