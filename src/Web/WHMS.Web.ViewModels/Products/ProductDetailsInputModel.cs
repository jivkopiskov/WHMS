namespace WHMS.Web.ViewModels.Products
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;

    public class ProductDetailsInputModel : IMapTo<Product>
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        [MaxLength(250)]
        public string ShortDescription { get; set; }

        [MaxLength(4000)]
        public string LongDescription { get; set; }

        [MaxLength(12)]
        public string UPC { get; set; }

        public decimal WebsitePrice { get; set; }

        public decimal WholesalePrice { get; set; }

        public decimal MAPPrice { get; set; }

        public decimal Cost { get; set; }

        public int? ConditionId { get; set; }

        [MaxLength(250)]
        public string LocationNotes { get; set; }

        public float Weight { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public float Lenght { get; set; }

        public int? BrandId { get; set; }

        public int? ManufacturerId { get; set; }

        public int? VendorId { get; set; }
    }
}
