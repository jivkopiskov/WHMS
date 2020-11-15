namespace WHMS.Web.ViewModels.Orders
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data;
    using WHMS.Web.ViewModels.ValidationAttributes;

    public class ShipOrderInputModel : IValidatableObject
    {
        public int OrderId { get; set; }

        public ShippingMethodInputModel ShippingMethod { get; set; }

        public IEnumerable<CarrierViewModel> Carriers { get; set; }

        [Required]
        [Display(Name = "Tracking number")]
        public string TrackingNumber { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var context = (WHMSDbContext)validationContext.GetService(typeof(WHMSDbContext));
            var order = context.Orders.Find(this.OrderId);
            if (order == null)
            {
                yield return new ValidationResult("Order doesn't exist");
            }

            if (order?.ShippingStatus == Data.Models.Orders.Enum.ShippingStatus.Shipped)
            {
                yield return new ValidationResult("Order already shipped");
            }

            if (order?.PaymentStatus != Data.Models.Orders.Enum.PaymentStatus.FullyCharged)
            {
                yield return new ValidationResult("Order is not fully charged. Cannot ship this order!");
            }
        }
    }
}
