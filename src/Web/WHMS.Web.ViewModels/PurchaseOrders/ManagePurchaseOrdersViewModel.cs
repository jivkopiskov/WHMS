namespace WHMS.Web.ViewModels.PurchaseOrders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ManagePurchaseOrdersViewModel : PagedViewModel
    {
        public IEnumerable<PurchaseOrderSummaryViewModel> PurchaseOrders { get; set; }
    }
}
