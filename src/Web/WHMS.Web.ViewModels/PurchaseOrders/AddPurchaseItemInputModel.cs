namespace WHMS.Web.ViewModels.PurchaseOrders
{
    using System.ComponentModel.DataAnnotations;

    using WHMS.Web.ViewModels.ValidationAttributes;

    public class AddPurchaseItemInputModel
    {
        [ValidProduct]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Qty { get; set; }

        [Range(0, int.MaxValue)]
        public decimal Cost { get; set; }
    }
}
