﻿namespace WHMS.Services.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using WHMS.Data;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;
    using WHMS.Web.ViewModels.Products;

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

        public async Task<int> CreateBrandAsync(string brandName)
        {
            var brand = new Brand() { Name = brandName };
            await this.context.Brands.AddAsync(brand);
            await this.context.SaveChangesAsync();
            return brand.Id;
        }

        public Task<int> CreateManufacturerAsync(string manufactuerName)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> CreateProductAsync(AddProductViewModel model)
        {
            var product = new Product()
            {
                SKU = model.SKU,
                AverageCost = model.AverageCost,
                ProductName = model.ProductName,
                ShortDescription = model.ShortDescription,
                MAPPrice = model.MAPPrice,
                Cost = model.Cost,
                WebsitePrice = model.WebsitePrice,
                WholesalePrice = model.WholesalePrice,
                Height = model.Height,
                Weight = model.Weight,
                Width = model.Width,
                Lenght = model.Lenght,
            };
            await this.context.Products.AddAsync(product);
            await this.context.SaveChangesAsync();
            return product.Id;
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

        public IEnumerable<Brand> GetAllBrands(int id = 0)
        {
            var brands = this.context.Brands
                .Where(x => x.Id > id)
                .Take(50)
                .ToList();
            return brands;
        }

        public IEnumerable<ManageProductsViewModel> GetAllProducts(int id)
        {
            var products =
                this.context.Products.Include(x => x.Brand)
                .Include(x => x.Images)
                .Where(x => x.Id >= id)
                .Take(50)
                .To<ManageProductsViewModel>()
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
