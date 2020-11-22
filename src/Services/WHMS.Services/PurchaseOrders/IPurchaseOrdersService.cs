namespace WHMS.Services.PurchaseOrders
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using WHMS.Web.ViewModels.PurchaseOrders;

    public interface IPurchaseOrdersService
    {
        IEnumerable<T> GetAllPurchaseOrders<T>(PurchaseOrdersFilterModel input);

        IEnumerable<T> GetAllVendors<T>(int page);

        IEnumerable<T> GetAllVendors<T>();

        int GetAllVendorsCount();

        int GetAllPurchaseOrdersCount();

        T GetVendorDetails<T>(int id);

        Task<int> GetPurchaseOrderDetails(int purchaseOrderId);

        Task<int> AddPurchaseOrderAsync<T>(T input);

        Task<int> EditPurchaseOrderAsync(int purchaseOrderId);

        Task<int> EditPurchaseItemAsync(int purchaseItemId);

        Task<int> AddPurchaseItemAsync(int purchaseOrderId, int productId);

        // remember to recalcualte order grand total
        Task<int> DeletePurchaseItemAsync(int purchaseItemId);

        Task<int> RecalculatePurchaseOrderTotal(int purchaseOrderId);

        Task<int> ReceiveWholePurchaseOrder(int purchaseOrderId);

        Task<int> ReceivePurchaseItem(int purchaseItemId);

        Task<int> AddVendorAsync<T>(T input);

        Task<int> EditVendorAsync(int vendorId);
    }
}
