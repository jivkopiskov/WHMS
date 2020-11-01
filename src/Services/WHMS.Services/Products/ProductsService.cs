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

        public async Task<int> AddProductConditionAsync<T>(T input)
        {
            var condition = this.mapper.Map<ProductCondition>(input);
            this.context.ProductConditions.Add(condition);
            await this.context.SaveChangesAsync();
            return condition.Id;
        }

        public async Task AddProductImageAsync<T>(T input)
        {
            var image = this.mapper.Map<Image>(input);
            if (this.context.Images.Where(i => i.ProductId == image.ProductId && i.IsPrimary == true).Count() == 0)
            {
                image.IsPrimary = true;
            }

            this.context.Images.Add(image);
            await this.context.SaveChangesAsync();
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
            var product = this.mapper.Map<Product>(model);
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

        public IEnumerable<T> GetProductImages<T>(int productId)
        {
            var productDetails = this.context.Images.Where(x => x.ProductId == productId).To<T>();
            return productDetails;
        }

        public async Task UpdateDefaultImageAsync(int imageId)
        {
            var image = this.context.Images.Find(imageId);
            image.IsPrimary = true;
            var otherImages = this.context.Images.Where(i => i.ProductId == image.ProductId && i.Id != image.Id && i.IsPrimary);
            foreach (var img in otherImages)
            {
                img.IsPrimary = false;
            }

            await this.context.SaveChangesAsync();
        }

        public Task<int> RecalculateAvailableInventory(int productId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> GetAllConditions<T>(int id)
        {
            var conditions = this.context.ProductConditions.To<T>();
            return conditions;
        }
    }
}
