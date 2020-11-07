namespace WHMS.Web.ViewModels.Orders
{
    using System.Collections.Generic;

    public class AddOrderItemsInputModel
    {
        public int OrderId { get; set; }

        public IEnumerable<OrderItemViewModel> OrderItems { get; set; }
    }
}
