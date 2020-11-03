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

        public async Task CreateWarehouseAsync<T>(T input)
        {
            var wh = this.mapper.Map<Warehouse>(input);
            this.context.Warehouses.Add(wh);
            await this.context.SaveChangesAsync();
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

        public IEnumerable<T> GetAllBrands<T>(int page = 1)
        {
            var brands = this.context.Brands
                .Skip((page - 1) * GlobalConstants.PageSize)
                .Take(GlobalConstants.PageSize)
                .To<T>()
                .ToList();
            return brands;
        }

        public IEnumerable<T> GetAllBrands<T>()
        {
            var brands = this.context.Brands
                .To<T>()
                .ToList();
            return brands;
        }

        public IEnumerable<T> GetAllManufacturers<T>(int page = 1)
        {
            var brands = this.context.Manufacturers
                .Skip((page - 1) * GlobalConstants.PageSize)
                .Take(GlobalConstants.PageSize)
                .To<T>()
                .ToList();
            return brands;
        }

        public IEnumerable<T> GetAllManufacturers<T>()
        {
            var brands = this.context.Manufacturers
                .To<T>()
                .ToList();
            return brands;
        }

        public IEnumerable<T> GetAllProducts<T>(FilterInputModel input)
        {
            IQueryable<Product> filteredList = this.FilterProducts(input);

            var products =
                filteredList
                .Skip((input.Page - 1) * GlobalConstants.PageSize)
                .Take(GlobalConstants.PageSize)
                .To<T>()
                .ToList();

            return products;
        }

        public IEnumerable<T> GetAllProducts<T>()
        {
            var products =
                this.context.Products
                .To<T>()
                .ToList();

            return products;
        }

        public int GetAllProductsCount(FilterInputModel input)
        {
            var filteredProducts = this.FilterProducts(input);
            return filteredProducts.Count();
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

        public int GetAllBrandsCount()
        {
            return this.context.Brands.Count();
        }

        public int GetAllManufacturersCount()
        {
            return this.context.Manufacturers.Count();
        }

        private IQueryable<Product> FilterProducts(FilterInputModel input)
        {
            var filteredList = this.context.Products.Where(x => x.IsDeleted == false);

            if (!string.IsNullOrEmpty(input.Keyword))
            {
                filteredList = filteredList.Where(x => x.SKU.Contains(input.Keyword) || x.ProductName.Contains(input.Keyword));
            }

            if (input.BrandId != null)
            {
                filteredList = filteredList.Where(x => x.BrandId == input.BrandId);
            }

            if (input.ManufacturerId != null)
            {
                filteredList = filteredList.Where(x => x.ManufacturerId == input.ManufacturerId);
            }

            if (input.ConditionId != null)
            {
                filteredList = filteredList.Where(x => x.ConditionId == input.ConditionId);
            }

            return filteredList;
        }
    }
}
