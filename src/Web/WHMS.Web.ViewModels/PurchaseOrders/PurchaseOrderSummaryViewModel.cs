namespace WHMS.Web.ViewModels.PurchaseOrders
{
    using WHMS.Data.Models.PurchaseOrders;
    using WHMS.Data.Models.PurchaseOrders.Enum;
    using WHMS.Services.Mapping;

    public class PurchaseOrderSummaryViewModel : IMapFrom<PurchaseOrder>
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal GrandTotal { get; set; }

        public PurchaseOrderStatus PurchaseOrderStatus { get; set; }

        public ReceivingStatus ReceivingStatus { get; set; }

        public string VendorName { get; set; }
    }
}
