namespace WHMS.Web.ViewModels.Products
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;

    public class AddProductInputModel : IMapTo<Product>, IValidatableObject
    {
        [Required]
        [MaxLength(100)]
        public string SKU { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [MaxLength(250)]
        [Display(Name = "Short Description")]
        [DataType(DataType.MultilineText)]
        public string ShortDescription { get; set; }

        [MaxLength(12)]
        public string UPC { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Website Price")]
        public decimal WebsitePrice { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Wholesale Price")]
        public decimal WholesalePrice { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "MAP Price")]
        public decimal MAPPrice { get; set; }

        [Range(0, int.MaxValue)]
        public decimal Cost { get; set; }

        [Range(0, int.MaxValue)]
        public float Weight { get; set; }

        [Range(0, int.MaxValue)]
        public float Width { get; set; }

        [Range(0, int.MaxValue)]
        public float Height { get; set; }

        [Range(0, int.MaxValue)]
        public float Lenght { get; set; }

        [Display(Name = "Brand")]
        public int? BrandId { get; set; }

        [Display(Name = "Manufacturer")]
        public int? ManufacturerId { get; set; }

        [Display(Name = "Condition")]
        public int? ConditionId { get; set; }

        [Display(Name = "Created By")]
        public string CreatedById { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.MAPPrice > this.WebsitePrice)
            {
                yield return new ValidationResult("The website price must be higher or eqaul to the MAP Price");
            }

            if (this.Cost > this.MAPPrice)
            {
                yield return new ValidationResult("The cost must lower or eqaul to the MAP Price");
            }
        }
    }
}
