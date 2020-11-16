namespace WHMS.Web.ViewModels.Products
{
    using System.Collections.Generic;

    public class ProductFilterInputModel
    {
        public int Page { get; set; } = 1;

        public string Keyword { get; set; }

        public ProductsSorting Sorting { get; set; }

        public int? BrandId { get; set; }

        public int? ManufacturerId { get; set; }

        public int? ConditionId { get; set; }

        public Dictionary<string, string> ToDictionary()
        {
            var dict = new Dictionary<string, string>();
            dict[nameof(this.Sorting)] = this.Sorting.ToString();

            if (!string.IsNullOrEmpty(this.Keyword))
            {
                dict[nameof(this.Keyword)] = this.Keyword;
            }

            if (this.BrandId != null)
            {
                dict[nameof(this.BrandId)] = this.BrandId.ToString();
            }

            if (this.BrandId != null)
            {
                dict[nameof(this.ManufacturerId)] = this.ManufacturerId.ToString();
            }

            if (this.ConditionId != null)
            {
                dict[nameof(this.ConditionId)] = this.ConditionId.ToString();
            }

            return dict;
        }
    }
}
