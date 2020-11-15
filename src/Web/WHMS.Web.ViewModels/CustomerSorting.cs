namespace WHMS.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public enum CustomerSorting
    {
        [Display(Name = "By Id")]
        Id = 0,

        [Display(Name = "By Id descending")]
        IdDesc = 1,

        [Display(Name = "By email")]
        Alphabetically = 2,

        [Display(Name = "By email descending")]
        AlphabeticallyDesc = 3,

        [Display(Name = "By Number of orders")]
        NumberOfOrders = 4,
    }
}
