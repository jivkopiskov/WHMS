namespace WHMS.Services.Orders
{
    using System.Threading.Tasks;

    using WHMS.Web.ViewModels.Orders;
    using WHMS.Web.ViewModels.Products;

    public interface IOrderItemsService
    {
        Task<int> AddOrderItemAsync(AddOrderItemsInputModel input);

        Task<int> AddOrderItemAsync(AddProductToOrderInputModel input);

        Task DeleteOrderItemAsync(int orderItemId);
    }
}
