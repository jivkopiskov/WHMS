namespace WHMS.Services.Products
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
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
        private readonly WHMSDbContext context;
        private readonly IInventoryService inventoryService;
        private readonly IMapper mapper;

        public ProductsService(WHMSDbContext context, IInventoryService inventoryService)
        {
            this.context = context;
            this.inventoryService = inventoryService;
            this.mapper = AutoMapperConfig.MapperInstance;
        }

        public async Task AddProductImageAsync<T>(T input)
        {
            var image = this.mapper.Map<Image>(input);
            if (!this.context.Images.Where(i => i.ProductId == image.ProductId && i.IsPrimary == true).Any())
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
            await this.inventoryService.RecalculateAvailableInventoryAsync(product.Id);
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

        public int GetAllProductsCount()
        {
            var filteredProducts = this.context.Products;
            return filteredProducts.Count();
        }

        public T GetProductDetails<T>(int productId)
        {
            var productDetails = this.context.Products.Where(x => x.Id == productId).To<T>().FirstOrDefault();
            return productDetails;
        }

        public IEnumerable<T> GetProductImages<T>(int productId)
        {
            var productImages = this.context.Images.Where(x => x.ProductId == productId).To<T>();
            return productImages;
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

        public async Task<string> ImportProductsAsync(Stream stream)
        {
            var sb = new StringBuilder();
            var dt = ExcelHelperClass.GetDataTableFromExcel(stream);

            var products = this.ConvertDatatableToProductInputEnumrable(dt);
            var invalidProducts = products.Where(x => !this.IsSkuAvailable(x.SKU));
            if (invalidProducts.Any())
            {
                sb.AppendLine($"Failed to create the following products due to duplicate SKUs: {string.Join(", ", invalidProducts.Select(x => x.SKU))}");
            }

            products = products.Except(invalidProducts);

            foreach (var product in products)
            {
                ICollection<ValidationResult> validationResults = new List<ValidationResult>();
                if (!ExcelHelperClass.TryValidate(product, out validationResults))
                {
                    sb.AppendLine(string.Join(Environment.NewLine, validationResults.Select(x => x.ErrorMessage)));
                    continue;
                }
                else
                {
                    var id = await this.CreateProductAsync(product);
                    var imageInput = new ImageViewModel { ProductId = id, Url = product.ImageURL };
                    await this.AddProductImageAsync(imageInput);
                }
            }

            return sb.ToString();
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

        private IEnumerable<AddProductInputModel> ConvertDatatableToProductInputEnumrable(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new AddProductInputModel
                {
                    SKU = row["SKU"].ToString(),
                    ProductName = row["ProductName"].ToString(),
                    ShortDescription = row["ShortDescription"].ToString(),
                    UPC = row["UPC"].ToString(),
                    WebsitePrice = Convert.ToDecimal(row["WebsitePrice"]),
                    WholesalePrice = Convert.ToDecimal(row["WholesalePrice"]),
                    MAPPrice = Convert.ToDecimal(row["MAPPrice"]),
                    Cost = Convert.ToDecimal(row["Cost"]),
                    ManufacturerId = Convert.ToInt32(row["ManufacturerId"]),
                    ConditionId = Convert.ToInt32(row["ConditionId"]),
                    BrandId = Convert.ToInt32(row["BrandId"]),
                    Weight = (float)Convert.ToDecimal(row["Weight"]),
                    Width = (float)Convert.ToDecimal(row["Width"]),
                    Height = (float)Convert.ToDecimal(row["Height"]),
                    Lenght = (float)Convert.ToDecimal(row["Lenght"]),
                    ImageURL = row["PrimaryImageURL"].ToString(),
                };
            }
        }
    }
}
