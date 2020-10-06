namespace WHMS.Data.Models.Products
{
    using System.Collections.Generic;

    using WHMS.Data.Common.Models;

    public class ProductCondition : BaseDeletableModel<int>
    {
        public string ConditionName { get; set; }

        public string ConditionDescription { get; set; }

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
