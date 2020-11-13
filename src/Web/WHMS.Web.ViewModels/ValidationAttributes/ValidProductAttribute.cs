namespace WHMS.Web.ViewModels.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using WHMS.Data;

    public class ValidProductAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (WHMSDbContext)validationContext.GetService(typeof(WHMSDbContext));
            int productId;
            if (!int.TryParse(value.ToString(), out productId))
            {
                return new ValidationResult("The productId entered is not a number");
            }

            if (context.Products.Any(x => x.Id == productId))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("There is no product with this Id");
        }
    }
}
