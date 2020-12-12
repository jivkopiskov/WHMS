namespace WHMS.Services.Products
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using WHMS.Web.ViewModels.Products;

    public interface IWarehouseService
    {
        Task CreateWarehouseAsync<T>(T input);

        IEnumerable<T> GetAllWarehouses<T>();

        IEnumerable<ProductWarehouseViewModel> GetProductWarehouseInfo(int productId);
    }
}
