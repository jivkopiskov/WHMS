namespace WHMS.Web.ViewModels.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using WHMS.Data;

    public class ValidOrderAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (WHMSDbContext)validationContext.GetService(typeof(WHMSDbContext));
            int id;
            if (!int.TryParse(value.ToString(), out id))
            {
                return new ValidationResult("An order with this id doesn't exist");
            }

            if (context.Orders.Any(x => x.Id == id))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("There is no order with this Id");
        }
    }
}
