namespace WHMS.Web.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ManageCustomersViewModel : PagedViewModel
    {
        public IEnumerable<CustomerViewModel> Customers { get; set; }

        public CustomersFilterInputModel Filters { get; set; }
    }
}
