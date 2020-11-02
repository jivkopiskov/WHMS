namespace WHMS.Web.ViewModels.Products
{
    using System.Collections.Generic;

    public class ManageManufacturersViewModel : PagedViewModel
    {
        public IEnumerable<ManufacturerViewModel> Manufacturers { get; set; }
    }
}
