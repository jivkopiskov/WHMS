namespace WHMS.Web.ViewModels.Products
{
    using System.Collections.Generic;

    public class ManageBrandsViewModel : PagedViewModel
    {
        public IEnumerable<BrandViewModel> Brands { get; set; }
    }
}
