namespace WHMS.Web.ViewModels.PurchaseOrders
{
    using WHMS.Data.Models.PurchaseOrders;
    using WHMS.Services.Mapping;

    public class PurchaseOrderSummaryViewModel : IMapFrom<PurchaseOrder>
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal GrandTotal { get; set; }
    }
}
