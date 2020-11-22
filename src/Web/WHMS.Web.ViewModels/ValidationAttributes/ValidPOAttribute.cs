namespace WHMS.Web.ViewModels.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using WHMS.Data;
    using WHMS.Data.Models.PurchaseOrder.Enum;

    public class ValidPOAttribute : ValidationAttribute
    {
        private readonly PurchaseOrderStatus[] purchaseOrderStatuses;

        public ValidPOAttribute(params PurchaseOrderStatus[] purchaseOrderStatus)
        {
            this.purchaseOrderStatuses = purchaseOrderStatus;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (WHMSDbContext)validationContext.GetService(typeof(WHMSDbContext));
            int id;
            if (!int.TryParse(value.ToString(), out id))
            {
                return new ValidationResult("An order with this id doesn't exist");
            }

            if (context.PurchaseOrders.Any(x => x.Id == id))
            {
                if (context.PurchaseOrders.Any(x => x.Id == id && this.purchaseOrderStatuses.Contains(x.PurchaseOrderStatus)))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult($"PO is not {string.Join(" or ", this.purchaseOrderStatuses)}. You can't modify it.");
                }
            }

            return new ValidationResult("There is no PO with this Id");
        }
    }
}
