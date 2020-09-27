namespace WHMS.Data.Models.Order
{
    using System.Collections.Generic;

    using WHMS.Data.Common.Models;

    public class Carrier : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public ICollection<ShippingMethod> ShippingMethods { get; set; }
    }
}
