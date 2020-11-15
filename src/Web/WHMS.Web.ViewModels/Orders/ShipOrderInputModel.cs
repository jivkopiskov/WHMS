namespace WHMS.Web.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using WHMS.Web.ViewModels.ValidationAttributes;

    public class ShipOrderInputModel
    {
        [ValidOrder]
        public int OrderId { get; set; }

        public ShippingMethodViewModel ShippingMethod { get; set; }

        public IEnumerable<CarrierViewModel> Carriers { get; set; }
    }
}
