namespace WHMS.Web.ViewModels.Products
{
    using System.Collections.Generic;

    public class ManageProductsViewModel : PagedViewModel
    {
        public IEnumerable<ProductsViewModel> Products { get; set; }
    }
}
