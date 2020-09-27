namespace WHMS.Data.Models.Order
{
    using WHMS.Data.Common.Models;
    using WHMS.Data.Models.Order.Enum;

    public class Payment : BaseDeletableModel<int>
    {
        public int OrderId { get; set; }

        public Order Order { get; set; }

        public PaymentType PaymentType { get; set; }
    }
}
