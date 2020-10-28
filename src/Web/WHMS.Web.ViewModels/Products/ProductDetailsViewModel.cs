namespace WHMS.Web.ViewModels.Products
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;

    public class ProductDetailsViewModel : IHaveCustomMappings
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string SKU { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        [MaxLength(250)]
        public string ShortDescription { get; set; }

        [MaxLength(4000)]
        public string LongDescription { get; set; }

        [MaxLength(12)]
        public string UPC { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedByEmail { get; set; }

        public decimal WebsitePrice { get; set; }

        public decimal WholesalePrice { get; set; }

        public decimal MAPPrice { get; set; }

        public decimal Cost { get; set; }

        public decimal LastCost { get; set; }

        public decimal AverageCost { get; set; }

        public string ConditionName { get; set; }

        [MaxLength(250)]
        public string LocationNotes { get; set; }

        public float Weight { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public float Lenght { get; set; }

        public string BrandName { get; set; }

        public string ManufacturerName { get; set; }

        public string VendorName { get; set; }

        public string DefaultImage { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductDetailsViewModel>().ForMember(
                            x => x.DefaultImage,
                            opt => opt.MapFrom(x => x.Images.FirstOrDefault().Url));
        }
    }
}
