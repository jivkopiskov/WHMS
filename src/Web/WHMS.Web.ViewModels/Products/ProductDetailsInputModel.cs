namespace WHMS.Web.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;

    using Validation = System.ComponentModel.DataAnnotations;

    public class ProductDetailsInputModel : IMapTo<Product>, IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        [Required]
        [MaxLength(250)]
        public string ShortDescription { get; set; }

        [MaxLength(4000)]
        public string LongDescription { get; set; }

        [MaxLength(12)]
        public string UPC { get; set; }

        [Range(0, int.MaxValue)]
        public decimal WebsitePrice { get; set; }

        [Range(0, int.MaxValue)]
        public decimal WholesalePrice { get; set; }

        [Range(0, int.MaxValue)]
        public decimal MAPPrice { get; set; }

        [Range(0, int.MaxValue)]
        public decimal Cost { get; set; }

        [MaxLength(250)]
        public string LocationNotes { get; set; }

        [Range(0, int.MaxValue)]
        public float Weight { get; set; }

        [Range(0, int.MaxValue)]
        public float Width { get; set; }

        [Range(0, int.MaxValue)]
        public float Height { get; set; }

        [Range(0, int.MaxValue)]
        public float Lenght { get; set; }

        public int? BrandId { get; set; }

        public int? ManufacturerId { get; set; }

        public int? ConditionId { get; set; }

        public int? VendorId { get; set; }

        public IEnumerable<ValidationResult> Validate(Validation.ValidationContext validationContext)
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
