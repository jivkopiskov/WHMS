namespace WHMS.Services.Orders
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using WHMS.Data.Models.Orders;
    using WHMS.Web.ViewModels.Orders;

    public interface IOrdersService
    {
        IEnumerable<T> GetAllOrders<T>(OrdersFilterInputModel input);

        T GetOrderDetails<T>(int orderId);

        Task<int> CreateOrderAsync(AddOrderInputModel input);

        Task<int> EditOrderAsync(int orderId);

        Task<int> AddOrderItemAsync(AddOrderItemsInputModel input);

        Task<int> EditOrderItemAsync(int orderItemId);

        // remember to recalcualte order grand total
        Task<int> DeleteOrderItemAsync(int orderItemId);

        Task RecalculateOrderTotal(int orderId);

        Task<int> ShipOrderAsync(int orderId, string shippingMethod, string trackingNumber);

        Task<int> AddCarrierAsync(string carrierName);

        Task<int> AddShippingMethodAsync(Carrier carrier, string shippingMethod);

        Task<int> CreateCustomerAsync();

        Task<int> EditCustomerAsync(int customerId);

        Task<int> GetCustomerOrdersAsync(int customerId);

        Task<int> AddPaymentAsync(int orderId, decimal amount);

        T GetCustomer<T>(string email);
    }
}
