namespace WHMS.Services.Orders
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using WHMS.Web.ViewModels.Orders;

    public interface IShippingService
    {
        Task ShipOrderAsync(ShipOrderInputModel input);

        Task UnshipOrderAsync(int orderId);

        Task<int> AddCarrierAsync(string carrierName);

        IEnumerable<T> GetAllCarriers<T>();

        IEnumerable<T> GetAllServicesForCarrier<T>(int carrierId);

        Task<int> AddShippingMethodAsync(int carrierId, string shippingMethod);

        Task DeleteShippingMethodAsync(int id);
    }
}
