namespace WHMS.Data.Models.Orders
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Common.Models;

    public class Carrier : BaseDeletableModel<int>
    {
        [MaxLength(20)]
        public string Name { get; set; }

        public ICollection<ShippingMethod> ShippingMethods { get; set; }
    }
}
