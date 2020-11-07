namespace WHMS.Web.ViewModels.Orders
{
    using System.ComponentModel.DataAnnotations;

    public class OrderItemViewModel
    {
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Qty { get; set; }
    }
}
