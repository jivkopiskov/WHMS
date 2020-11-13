namespace WHMS.Web.ViewModels.Orders
{
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Models.Orders;
    using WHMS.Services.Mapping;
    using WHMS.Web.ViewModels.ValidationAttributes;

    public class AddOrderItemViewModel : IMapFrom<OrderItem>
    {
        [ValidProduct]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Qty { get; set; }

        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }
    }
}
