namespace WHMS.Web.ViewModels.Products
{
    using System.Collections.Generic;

    public class ProductImagesViewModel
    {
        public int ProductId { get; set; }

        public IEnumerable<ImageViewModel> Images { get; set; }
    }
}
