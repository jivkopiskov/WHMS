namespace WHMS.Web.ViewModels.PurchaseOrders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AddPurchaseItemsInputModel
    {
        public int PurchaseOrderId { get; set; }

        public int VendorId { get; set; }

        public IEnumerable<AddPurchaseItemInputModel> PurchaseItems { get; set; }
    }
}
