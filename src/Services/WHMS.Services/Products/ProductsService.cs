namespace WHMS.Services.Products
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using WHMS.Common;
    using WHMS.Data;
    using WHMS.Data.Models.Orders.Enum;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;
    using WHMS.Web.ViewModels;
    using WHMS.Web.ViewModels.Products;
    using WHMS.Web.ViewModels.Products.Enums;

    public class ProductsService : IProductsService
    {
        private WHMSDbContext context;
        private IMapper mapper;

        public ProductsService(WHMSDbContext context)
        {
            this.context = context;
            this.mapper = AutoMapperConfig.MapperInstance;
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

        public async Task<int> CreateProductAsync(AddProductInputModel model)
        {
            var product = this.mapper.Map<Product>(model);
            await this.context.Products.AddAsync(product);
            await this.context.SaveChangesAsync();
            return product.Id;
        }

        public bool IsSkuAvailable(string sku)
        {
            return !this.context.Products.Any(x => x.SKU == sku);
        }

        public bool IsValidProductId(int id)
        {
            return this.context.Products.Any(x => x.Id == id);
        }

        public Task<int> DeleteProductImageAsync(int imageId)
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

        public IEnumerable<T> GetAllProducts<T>(ProductFilterInputModel input)
        {
            var filteredList = this.FilterProducts(input);

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

        public int GetAllProductsCount(ProductFilterInputModel input)
        {
            var filteredProducts = this.FilterProducts(input);
            return filteredProducts.Count();
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

        private IQueryable<Product> FilterProducts(ProductFilterInputModel input)
        {
            var filteredList = this.context.Products.Where(x => x.IsDeleted == false);
            filteredList = input.Sorting switch
            {
                ProductsSorting.Id => filteredList.OrderBy(p => p.Id),
                ProductsSorting.IdDesc => filteredList.OrderByDescending(p => p.Id),
                ProductsSorting.Alphabetically => filteredList.OrderBy(p => p.SKU),
                ProductsSorting.AlphabeticallyDesc => filteredList.OrderByDescending(p => p.SKU),
                ProductsSorting.Price => filteredList.OrderBy(p => p.WebsitePrice),
                ProductsSorting.PriceDesc => filteredList.OrderByDescending(p => p.WebsitePrice),
                _ => filteredList,
            };
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
