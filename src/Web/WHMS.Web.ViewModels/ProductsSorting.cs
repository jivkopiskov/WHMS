namespace WHMS.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public enum ProductsSorting
    {
        [Display(Name = "By Id")]
        Id = 0,

        [Display(Name = "By Id descending")]
        IdDesc = 1,

        [Display(Name = "By SKU")]
        Alphabetically = 2,

        [Display(Name = "By SKU Descending")]
        AlphabeticallyDesc = 3,

        [Display(Name = "By Price")]
        Price = 4,

        [Display(Name = "By Price Descending")]
        PriceDesc = 5,
    }
}
