namespace WHMS.Data.Models.Products
{
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Common.Models;

    public class Image : BaseDeletableModel<int>
    {
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [MaxLength(2048)]
        public string Url { get; set; }

        public bool IsPrimary { get; set; }
    }
}
