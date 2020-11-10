namespace WHMS.Services.Products
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using WHMS.Data.Models.Products;

    using WHMS.Web.ViewModels.Products;

    public interface IProductsService
    {
        Task<int> CreateProductAsync(AddProductInputModel model);

        bool IsSkuAvailable(string sku);

        bool IsValidProductId(int id);

        Task AddProductImageAsync<T>(T input);

        Task<int> DeleteProductImageAsync(int imageId);

        Task<T> EditProductAsync<T, TInput>(TInput model);

        T GetProductDetails<T>(int productId);

        IEnumerable<T> GetProductImages<T>(int productId);

        Task UpdateDefaultImageAsync(int imageId);

        IEnumerable<T> GetAllProducts<T>(ProductFilterInputModel input);

        IEnumerable<T> GetAllProducts<T>();

        public int GetAllProductsCount(ProductFilterInputModel input);
    }
}
