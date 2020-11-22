namespace WHMS.Web.ViewModels.PurchaseOrders
{
    using System.Collections.Generic;

    using WHMS.Data.Models.PurchaseOrder.Enum;
    using WHMS.Web.ViewModels.ValidationAttributes;

    public class AddPurchaseItemsInputModel
    {
        [ValidPO(PurchaseOrderStatus.Created)]
        public int PurchaseOrderId { get; set; }

        public int VendorId { get; set; }

        public IEnumerable<AddPurchaseItemInputModel> PurchaseItems { get; set; }
    }
}
