namespace WHMS.Web.ViewModels.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using WHMS.Data;

    public class ValidBrandAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (WHMSDbContext)validationContext.GetService(typeof(WHMSDbContext));
            int id;
            if (!int.TryParse(value.ToString(), out id))
            {
                return new ValidationResult("The brandId entered is not a number");
            }

            if (context.Brands.Any(x => x.Id == id))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("There is no brand with this Id");
        }
    }
}
