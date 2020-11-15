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

        Task<int> AddOrderAsync(AddOrderInputModel input);

        Task<int> EditOrderAsync(int orderId);

        Task<int> AddOrderItemAsync(AddOrderItemsInputModel input);

        Task<int> EditOrderItemAsync(int orderItemId);

        // remember to recalcualte order grand total
        Task<int> DeleteOrderItemAsync(int orderItemId);

        Task CancelOrderAsync(int orderId);

        Task SetInProcessAsync(int orderId);

        Task RecalculateOrderTotal(int orderId);

        Task RecalculateOrderReservesAsync(int orderId);

        Task<int> ShipOrderAsync(ShipOrderInputModel input);

        Task<int> AddCarrierAsync(string carrierName);

        IEnumerable<T> GetAllCarriers<T>();

        IEnumerable<T> GetAllServicesForCarrier<T>(int carrierId);

        Task<int> AddShippingMethodAsync(Carrier carrier, string shippingMethod);

        Task<int> CreateCustomerAsync();

        Task<int> EditCustomerAsync(int customerId);

        Task<int> GetCustomerOrdersAsync(int customerId);

        Task AddPaymentAsync<T>(T input);

        IEnumerable<T> GetAllPayments<T>(int orderId);

        Task DeletePaymentAsync(int paymentId);

        Task RecalculatePaymentStatusAsync(int orderId);

        T GetCustomer<T>(string email);
    }
}
