namespace WHMS.Services.Products
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using WHMS.Data.Models.Products;

    using WHMS.Web.ViewModels.Products;

    public interface IProductsService
    {
        Task<int> CreateProductAsync(AddProductViewModel model);

        Task AddProductImageAsync<T>(T input);

        Task<int> DeleteProductImageAsync(int imageId);

        Task<int> AddProductConditionAsync<T>(T input);

        Task<int> EditProductCondition(int conditionId, string conditionName, string conditionDescription);

        Task<T> EditProductAsync<T, TInput>(TInput model);

        T GetProductDetails<T>(int productId);

        IEnumerable<T> GetProductImages<T>(int productId);

        Task UpdateDefaultImageAsync(int imageId);

        Task<int> GetProductAvailableInventory(int productId);

        Task<int> RecalculateAvailableInventory(int productId);

        IEnumerable<T> GetAllProducts<T>(int page);

        IEnumerable<T> GetAllProducts<T>();

        public int GetAllProductsCount();

        Task<int> CreateBrandAsync(string brandName);

        Task<int> EditBrandAsync(int brandId);

        IEnumerable<T> GetAllBrands<T>();

        IEnumerable<T> GetAllBrands<T>(int page);

        int GetAllBrandsCount();

        Task<int> CreateManufacturerAsync(string manufactuerName);

        Task<int> EditManufactuerAsync(int manufactuerId);

        IEnumerable<T> GetAllManufacturers<T>(int page);

        IEnumerable<T> GetAllManufacturers<T>();

        int GetAllManufacturersCount();

        Task<int> CreateWarehouseAsync(string warehouseName, bool isSellable);

        Task<int> EditWarehouseAsync(int warehouseId, bool isSellable);

        IEnumerable<T> GetAllWarehouses<T>();

        IEnumerable<T> GetAllConditions<T>(int id = 0);
    }
}
