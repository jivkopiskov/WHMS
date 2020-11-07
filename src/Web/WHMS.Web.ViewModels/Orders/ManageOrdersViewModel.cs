namespace WHMS.Web.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ManageOrdersViewModel
    {
        public IEnumerable<OrdersViewModel> Orders { get; set; }

        public OrdersFilterInputModel Filters { get; set; }
    }
}
