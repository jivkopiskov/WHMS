namespace WHMS.Web.ViewModels.Orders
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Models.Orders;
    using WHMS.Data.Models.Orders.Enum;
    using WHMS.Services.Mapping;
    using WHMS.Web.ViewModels.ValidationAttributes;

    public class PaymentInputModel : IMapTo<Payment>, IMapFrom<Payment>
    {
        [ValidOrder]
        public int OrderId { get; set; }

        [Display(Name = "Payment type")]
        public PaymentType PaymentType { get; set; }

        [Display(Name = "Payment amount")]
        [Range(0.01, int.MaxValue)]
        public decimal Amount { get; set; }
    }
}
