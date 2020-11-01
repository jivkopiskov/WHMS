namespace WHMS.Web.ViewModels.Products
{
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;

    public class ImageViewModel : IMapFrom<Image>
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        [MaxLength(2048)]
        public string Url { get; set; }

        public bool IsPrimary { get; set; }
    }
}
