﻿namespace WHMS.Services.Products
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using WHMS.Data;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;
    using WHMS.Web.ViewModels.Products;

    public class WarehouseService : IWarehouseService
    {
        private WHMSDbContext context;
        private IMapper mapper;

        public WarehouseService(WHMSDbContext context)
        {
            this.context = context;
            this.mapper = AutoMapperConfig.MapperInstance;
        }

        public async Task CreateWarehouseAsync<T>(T input)
        {
            var wh = this.mapper.Map<Warehouse>(input);
            this.context.Warehouses.Add(wh);
            await this.context.SaveChangesAsync();
        }

        public Task<int> EditWarehouseAsync(int warehouseId, bool isSellable)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> GetAllWarehouses<T>()
        {
            return this.context.Warehouses
                .To<T>()
                .ToList();
        }

        public IEnumerable<ProductWarehouseViewModel> GetProductWarehouseInfo(int productId)
        {
            return this.context.Warehouses
                .Select(x => new ProductWarehouseViewModel
                {
                    ProductId = productId,
                    ProductSKU = this.context.Products.Find(productId).SKU,
                    WarehouseName = x.Name,
                    WarehouseIsSellable = x.IsSellable,
                    TotalPhysicalQuanitity = this.context.ProductWarehouses.FirstOrDefault(pw => pw.WarehouseId == x.Id && pw.ProductId == productId) == null ? 0 : this.context.ProductWarehouses.FirstOrDefault(pw => pw.WarehouseId == x.Id && pw.ProductId == productId).TotalPhysicalQuanitiy,
                    AggregateQuantity = this.context.ProductWarehouses.FirstOrDefault(pw => pw.WarehouseId == x.Id && pw.ProductId == productId) == null ? 0 : this.context.ProductWarehouses.FirstOrDefault(pw => pw.WarehouseId == x.Id && pw.ProductId == productId).AggregateQuantity,
                    ReservedQuantity = this.context.ProductWarehouses.FirstOrDefault(pw => pw.WarehouseId == x.Id && pw.ProductId == productId) == null ? 0 : this.context.ProductWarehouses.FirstOrDefault(pw => pw.WarehouseId == x.Id && pw.ProductId == productId).ReservedQuantity,
                })
                .ToList();
        }
    }
}