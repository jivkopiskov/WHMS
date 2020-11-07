namespace WHMS.Web.ViewModels.Products
{
    using System.Collections.Generic;

    public class ProductFilterInputModel
    {
        public int Page { get; set; } = 1;

        public string Keyword { get; set; }

        public int? BrandId { get; set; }

        public int? ManufacturerId { get; set; }

        public int? ConditionId { get; set; }

        public Dictionary<string, string> ToDictionary()
        {
            var dict = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(this.Keyword))
            {
                dict["keyword"] = this.Keyword;
            }

            if (this.BrandId != null)
            {
                dict["brandId"] = this.BrandId.ToString();
            }

            if (this.BrandId != null)
            {
                dict["manufacturerId"] = this.ManufacturerId.ToString();
            }

            if (this.ConditionId != null)
            {
                dict["conditionId"] = this.ConditionId.ToString();
            }

            return dict;
        }
    }
}
