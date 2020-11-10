namespace WHMS.Web.ViewModels.Products
{
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;

    public class AddImagesInputModel : IMapTo<Image>
    {
        public int ProductId { get; set; }

        public string[] URL { get; set; }
    }
}
