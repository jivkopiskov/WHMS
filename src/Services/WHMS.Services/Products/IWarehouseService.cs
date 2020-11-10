namespace WHMS.Services.Products
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using WHMS.Web.ViewModels.Products;

    public interface IWarehouseService
    {
        Task CreateWarehouseAsync<T>(T input);

        Task<int> EditWarehouseAsync(int warehouseId, bool isSellable);

        IEnumerable<T> GetAllWarehouses<T>();

        IEnumerable<ProductWarehouseViewModel> GetProductWarehouseInfo(int productId);
    }
}
