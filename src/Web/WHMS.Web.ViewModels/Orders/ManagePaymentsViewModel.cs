namespace WHMS.Web.ViewModels.Orders
{
    using System.Collections.Generic;

    using WHMS.Web.ViewModels.ValidationAttributes;

    public class ManagePaymentsViewModel
    {
        [ValidOrder]
        public int OrderId { get; set; }

        public decimal OrderGrandTotal { get; set; }

        public IEnumerable<PaymentViewModel> Payments { get; set; }
    }
}
