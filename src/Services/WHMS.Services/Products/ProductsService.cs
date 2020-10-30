namespace WHMS.Services.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using WHMS.Common;
    using WHMS.Data;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;
    using WHMS.Web.ViewModels.Products;

    public class ProductsService : IProductsService
    {
        private WHMSDbContext context;
        private IMapper mapper;

        public ProductsService(WHMSDbContext context)
        {
            this.context = context;
            this.mapper = AutoMapperConfig.MapperInstance;
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

        public async Task<int> CreateManufacturerAsync(string manufactuerName)
        {
            var manufacturer = new Manufacturer() { Name = manufactuerName };
            await this.context.Manufacturers.AddAsync(manufacturer);
            await this.context.SaveChangesAsync();
            return manufacturer.Id;
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

        public async Task<T> EditProductAsync<T, TInput>(TInput input)
        {
            var product = this.mapper.Map<TInput, Product>(input);
            var dbProduct = this.context.Products.Find(product.Id);

            // dbProduct.BrandId = product.BrandId;
            // dbProduct.ConditionId = product.ConditionId;
            // dbProduct.Cost = product.Cost;
            // dbProduct.Height = product.Height;
            // dbProduct.Lenght = product.Lenght;
            // dbProduct.Width = product.Width;
            // dbProduct.LocationNotes = product.LocationNotes;
            // dbProduct.ShortDescription = product.ShortDescription;
            // dbProduct.LongDescription = product.LongDescription;
            // dbProduct.ManufacturerId = product.ManufacturerId;
            // dbProduct.MAPPrice = product.MAPPrice;
            // dbProduct.ProductName = product.ProductName;
            // dbProduct.UPC = product.UPC;
            this.mapper.Map<TInput, Product>(input, dbProduct);
            await this.context.SaveChangesAsync();

            return this.mapper.Map<Product, T>(product);
        }

        public Task<int> EditProductCondition(int conditionId, string conditionName, string conditionDescription)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> EditWarehouseAsync(int warehouseId, bool isSellable)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> GetAllBrands<T>(int id = 0)
        {
            var brands = this.context.Brands
                .Where(x => x.Id > id)
                .Take(50)
                .To<T>()
                .ToList();
            return brands;
        }

        public IEnumerable<T> GetAllManufacturers<T>(int id = 0)
        {
            var brands = this.context.Manufacturers
                .Where(x => x.Id > id)
                .Take(50)
                .To<T>()
                .ToList();
            return brands;
        }

        public IEnumerable<T> GetAllProducts<T>(int id)
        {
            var products =
                this.context.Products
                .Where(x => x.Id >= id)
                .Take(GlobalConstants.PageSize)
                .To<T>()
                .ToList();

            return products;
        }

        public int GetAllProductsCount()
        {
            return this.context.Products.Count();
        }

        public IEnumerable<T> GetAllWarehouses<T>()
        {
            return this.context.Warehouses
                .To<T>()
                .ToList();
        }

        public Task<int> GetProductAvailableInventory(int productId)
        {
            throw new System.NotImplementedException();
        }

        public T GetProductDetails<T>(int productId)
        {
            var productDetails = this.context.Products.Where(x => x.Id == productId).To<T>().FirstOrDefault();
            return productDetails;
        }

        public Task<int> RecalculateAvailableInventory(int productId)
        {
            throw new System.NotImplementedException();
        }
    }
}
