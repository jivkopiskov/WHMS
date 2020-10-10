namespace WHMS.Services.PurchaseOrders
{
    using System.Threading.Tasks;

    public interface IPurchaseOrdersService
    {
        // id for pagination
        Task<int> GetAllPurchaseOrders(int purchaseOrderId);

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

        Task<int> CreateVendorAsync(string vendorName);

        Task<int> EditVendorAsync(int vendorId);
    }
}
