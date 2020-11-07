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

        IEnumerable<ProductWarehouseViewModel> GetProductWarehouseInfo(int productId);

        Task AddProductImageAsync<T>(T input);

        Task<int> DeleteProductImageAsync(int imageId);

        Task<int> AddProductConditionAsync<T>(T input);

        Task<int> EditProductCondition(int conditionId, string conditionName, string conditionDescription);

        Task<T> EditProductAsync<T, TInput>(TInput model);

        T GetProductDetails<T>(int productId);

        IEnumerable<T> GetProductImages<T>(int productId);

        Task UpdateDefaultImageAsync(int imageId);

        Task<bool> AdjustInventory(ProductAdjustmentInputModel input);

        int GetProductAvailableInventory(int productId);

        Task RecalculateAvailableInventory(int productId);

        IEnumerable<T> GetAllProducts<T>(ProductFilterInputModel input);

        IEnumerable<T> GetAllProducts<T>();

        public int GetAllProductsCount(ProductFilterInputModel input);

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

        Task CreateWarehouseAsync<T>(T input);

        Task<int> EditWarehouseAsync(int warehouseId, bool isSellable);

        IEnumerable<T> GetAllWarehouses<T>();

        IEnumerable<T> GetAllConditions<T>(int id = 0);
    }
}
