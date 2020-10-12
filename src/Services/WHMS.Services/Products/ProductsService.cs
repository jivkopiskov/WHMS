namespace WHMS.Services.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using WHMS.Data;
    using WHMS.Data.Models.Products;

    public class ProductsService : IProductsService
    {
        private WHMSDbContext context;

        public ProductsService(WHMSDbContext context)
        {
            this.context = context;
        }

        public Task<int> AddProductCondition(string conditionName, string conditionDescription)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> AddProductImageAsync(int productId, string imageURL)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> CreateBrandAsync(string brandName)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> CreateManufacturerAsync(string manufactuerName)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> CreateProductAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<int> CreateWarehouseAsync(string warehouseName, bool isSellable)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> DeleteProductImageAsync(int imageId)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> EditBrandAsync(int brandId)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> EditManufactuerAsync(int manufactuerId)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> EditProductAsync(int productId)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> EditProductCondition(int conditionId, string conditionName, string conditionDescription)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> EditWarehouseAsync(int warehouseId, bool isSellable)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Product> GetAllProducts(int id)
        {
            var products =
                this.context.Products.Include(x => x.Brand)
                .Include(x => x.Images)
                .Where(x => x.Id >= id)
                .Take(50)
                .ToList();

            return products;
        }

        public Task<int> GetProductAvailableInventory(int productId)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> GetProductDetails(int productId)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> RecalculateAvailableInventory(int productId)
        {
            throw new System.NotImplementedException();
        }
    }
}
