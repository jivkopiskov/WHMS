namespace WHMS.Services.PurchaseOrders
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPurchaseOrdersService
    {
        // id for pagination
        Task<int> GetAllPurchaseOrders(int purchaseOrderId);

        IEnumerable<T> GetAllVendors<T>(int page);

        IEnumerable<T> GetAllVendors<T>();

        int GetAllVendorsCount();

        T GetVendorDetails<T>(int id);

        Task<int> GetPurchaseOrderDetails(int purchaseOrderId);

        Task<int> CreatePurchaseOrderAsync();

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
