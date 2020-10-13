namespace WHMS.Web.ViewModels.Products
{
    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;

    public class ManufacturerViewModel : IMapFrom<Manufacturer>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
