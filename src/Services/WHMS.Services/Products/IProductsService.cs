namespace WHMS.Services.Products
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using WHMS.Data.Models.Products;

    using WHMS.Web.ViewModels.Products;

    public interface IProductsService
    {
        Task<int> CreateProductAsync(AddProductViewModel model);

        Task<int> AddProductImageAsync(int productId, string imageURL);

        Task<int> DeleteProductImageAsync(int imageId);

        Task<int> AddProductCondition(string conditionName, string conditionDescription);

        Task<int> EditProductCondition(int conditionId, string conditionName, string conditionDescription);

        Task<int> EditProductAsync(int productId);

        Task<int> GetProductDetails(int productId);

        Task<int> GetProductAvailableInventory(int productId);

        Task<int> RecalculateAvailableInventory(int productId);

        // pagination with SelectTop50 where id > lastIdReceived
        IEnumerable<T> GetAllProducts<T>(int id);

        public int GetAllProductsCount();

        Task<int> CreateBrandAsync(string brandName);

        Task<int> EditBrandAsync(int brandId);

        IEnumerable<T> GetAllBrands<T>(int id);

        Task<int> CreateManufacturerAsync(string manufactuerName);

        Task<int> EditManufactuerAsync(int manufactuerId);

        IEnumerable<T> GetAllManufacturers<T>(int id);

        Task<int> CreateWarehouseAsync(string warehouseName, bool isSellable);

        Task<int> EditWarehouseAsync(int warehouseId, bool isSellable);

        IEnumerable<T> GetAllWarehouses<T>();
    }
}
