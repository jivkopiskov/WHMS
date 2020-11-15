namespace WHMS.Web.ViewModels.Orders
{
    using System.Collections.Generic;

    public class ManageShippingMethodsViewModel
    {
        public IEnumerable<ShippingMethodViewModel> Methods { get; set; }

        public int CarrierId { get; set; }

        public string CarrierName { get; set; }
    }
}
