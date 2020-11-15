namespace WHMS.Services.Products
{
    using System.Threading.Tasks;

    using WHMS.Web.ViewModels.Products;

    public interface IInventoryService
    {
        Task<bool> AdjustInventory(ProductAdjustmentInputModel input);

        int GetProductAvailableInventory(int productId);

        Task RecalculateAvailableInventoryAsync(int productId);

        Task RecalculateReservedInventory(int productId, int warehouseId);
    }
}
