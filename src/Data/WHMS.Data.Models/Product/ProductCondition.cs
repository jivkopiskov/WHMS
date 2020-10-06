namespace WHMS.Data.Models.Products
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Common.Models;

    public class ProductCondition : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(30)]
        public string ConditionName { get; set; }

        [MaxLength(250)]
        public string ConditionDescription { get; set; }

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
