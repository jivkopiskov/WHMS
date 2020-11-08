namespace WHMS.Web.ViewModels.Orders
{
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Models.Orders;
    using WHMS.Services.Mapping;

    public class AddOrderItemViewModel : IMapFrom<OrderItem>
    {
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Qty { get; set; }

        public decimal Price { get; set; }
    }
}
