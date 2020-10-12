namespace WHMS.Web.ViewModels.Products
{
    using System.ComponentModel.DataAnnotations;

    public class AddProductViewModel
    {
        [Required]
        [MaxLength(100)]
        public string SKU { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        [MaxLength(250)]
        public string ShortDescription { get; set; }

        [MaxLength(12)]
        public string UPC { get; set; }

        public decimal WebsitePrice { get; set; } = 0;

        public decimal WholesalePrice { get; set; } = 0;

        public decimal MAPPrice { get; set; } = 0;

        public decimal Cost { get; set; } = 0;

        public decimal LastCost { get; set; } = 0;

        public decimal AverageCost { get; set; } = 0;

        public float Weight { get; set; } = 0;

        public float Width { get; set; } = 0;

        public float Height { get; set; } = 0;

        public float Lenght { get; set; } = 0;
    }
}
