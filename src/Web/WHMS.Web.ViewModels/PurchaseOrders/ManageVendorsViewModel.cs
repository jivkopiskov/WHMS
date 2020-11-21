namespace WHMS.Web.ViewModels.PurchaseOrders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ManageVendorsViewModel : PagedViewModel
    {
        public IEnumerable<AddVendorViewModel> Vendors { get; set; }
    }
}
