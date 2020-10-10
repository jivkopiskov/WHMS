namespace WHMS.Data.Models.Orders
{
    using WHMS.Data.Common.Models;
    using WHMS.Data.Models.Orders.Enum;

    public class Payment : BaseDeletableModel<int>
    {
        public int OrderId { get; set; }

        public Order Order { get; set; }

        public PaymentType PaymentType { get; set; }

        public decimal Amount { get; set; }
    }
}
