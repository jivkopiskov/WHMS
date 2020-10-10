namespace WHMS.Services.Orders
{
    using System.Threading.Tasks;

    using WHMS.Data.Models.Orders;

    public interface IOrdersService
    {
        // id for pagination
        Task<int> GetAllOrders(int orderId);

        Task<int> GetOrderDetails(int orderId);

        Task<int> CreateOrderAsync();

        Task<int> EditOrderAsync(int orderId);

        Task<int> AddOrderItemAsync(int productId);

        Task<int> EditOrderItemAsync(int orderItemId);

        // remember to recalcualte order grand total
        Task<int> DeleteOrderItemAsync(int orderItemId);

        Task<int> RecalculateOrderTotal(int orderId);

        Task<int> ShipOrderAsync(int orderId, string shippingMethod, string trackingNumber);

        Task<int> AddCarrierAsync(string carrierName);

        Task<int> AddShippingMethodAsync(Carrier carrier, string shippingMethod);

        Task<int> CreateCustomerAsync();

        Task<int> EditCustomerAsync(int customerId);

        Task<int> GetCustomerOrdersAsync(int customerId);

        Task<int> AddPaymentAsync(int orderId, decimal amount);
    }
}
