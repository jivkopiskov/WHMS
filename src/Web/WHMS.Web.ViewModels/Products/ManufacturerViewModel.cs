namespace WHMS.Web.ViewModels.Products
{
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;

    public class ManufacturerViewModel : IMapFrom<Manufacturer>
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
