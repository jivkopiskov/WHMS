﻿namespace WHMS.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using WHMS.Data.Common.Models;

    public class Product : BaseDeletableModel<int>
    {
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

        public decimal WebsitePrice { get; set; }

        public decimal WholesalePrice { get; set; }

        public decimal MAPPrice { get; set; }

        public decimal Cost { get; set; }

        public decimal LastCost { get; set; }

        public decimal AverageCost { get; set; }

        public int? ConditionId { get; set; }

        public ProductCondition Condition { get; set; }

        [MaxLength(250)]
        public string LocationNotes { get; set; }

        public float Weight { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public float Lenght { get; set; }

        public int? BrandId { get; set; }

        public Brand Brand { get; set; }

        public int? ManufacturerId { get; set; }

        public Manufacturer Manufacturer { get; set; }
    }
}
