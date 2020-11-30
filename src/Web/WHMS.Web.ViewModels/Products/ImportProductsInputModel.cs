namespace WHMS.Web.ViewModels.Products
{
    using Microsoft.AspNetCore.Http;

    public class ImportProductsInputModel
    {
        public IFormFile File { get; set; }
    }
}
