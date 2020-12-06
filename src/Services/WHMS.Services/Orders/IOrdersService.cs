namespace WHMS.Services.Orders
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using WHMS.Data.Models.Orders;
    using WHMS.Web.ViewModels.Orders;
    using WHMS.Web.ViewModels.Products;

    public interface IOrdersService
    {
        IEnumerable<T> GetAllOrders<T>(OrdersFilterInputModel input);

        int GetAllOrdersCount();

        T GetOrderDetails<T>(int orderId);

        Task<int> AddOrderAsync(AddOrderInputModel input);

        Task DeleteOrderAsync(int orderId);

        Task CancelOrderAsync(int orderId);

        Task SetInProcessAsync(int orderId);

        Task RecalculateOrderTotal(int orderId);

        Task RecalculateOrderReservesAsync(int orderId);

        Task AddPaymentAsync<T>(T input);

        IEnumerable<T> GetAllPayments<T>(int orderId);

        Task DeletePaymentAsync(int paymentId);

        Task RecalculatePaymentStatusAsync(int orderId);

        Task RecalculateOrderStatusesAsync(int orderId);
    }
}
