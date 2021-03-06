﻿namespace WHMS.Web.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;

    public class ProductDetailsViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }

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

        [MaxLength(4000)]
        [Display(Name = "Long Description")]
        public string LongDescription { get; set; }

        [MaxLength(12)]
        public string UPC { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Created By")]
        public string CreatedByEmail { get; set; }

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
        [Display(Name = "Last Cost")]
        public decimal LastCost { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Average Cost")]
        public decimal AverageCost { get; set; }

        [MaxLength(250)]
        [Display(Name = "Location notes")]
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

        public string DefaultImage { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductDetailsViewModel>().ForMember(
                            x => x.DefaultImage,
                            opt => opt.MapFrom(x => x.Images.Where(i => i.IsPrimary).FirstOrDefault().Url));
        }
    }
}
