﻿namespace WHMS.Services.Products
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using WHMS.Data.Models.Products;

    using WHMS.Web.ViewModels.Products;

    public interface IProductsService
    {
        Task<int> CreateProductAsync(AddProductViewModel model);

        Task<int> EditProductAsync(int productId);

        Task<int> GetProductDetails(int productId);

        // pagination with SelectTop50 where id > lastIdReceived
        IEnumerable<ManageProductsViewModel> GetAllProducts(int id);

        Task<int> CreateBrandAsync(string brandName);

        Task<int> CreateManufacturerAsync(string manufactuerName);

        Task<int> EditBrandAsync(int brandId);

        Task<int> EditManufactuerAsync(int manufactuerId);

        Task<int> GetProductAvailableInventory(int productId);

        Task<int> AddProductImageAsync(int productId, string imageURL);

        Task<int> DeleteProductImageAsync(int imageId);

        Task<int> AddProductCondition(string conditionName, string conditionDescription);

        Task<int> EditProductCondition(int conditionId, string conditionName, string conditionDescription);

        Task<int> CreateWarehouseAsync(string warehouseName, bool isSellable);

        Task<int> EditWarehouseAsync(int warehouseId, bool isSellable);

        Task<int> RecalculateAvailableInventory(int productId);

        IEnumerable<Brand> GetAllBrands(int id);
    }
}