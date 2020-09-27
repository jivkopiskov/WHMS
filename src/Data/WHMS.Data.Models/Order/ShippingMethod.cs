namespace WHMS.Data.Models.Order
{
    using WHMS.Data.Common.Models;

    public class ShippingMethod : BaseDeletableModel<int>
    {
        public Carrier Carrier { get; set; }

        public string Name { get; set; }
    }
}
