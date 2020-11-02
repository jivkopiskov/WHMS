namespace WHMS.Web.ViewModels.Products
{
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Models;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;

    public class WarehouseViewModel : IMapFrom<Warehouse>, IMapTo<Warehouse>
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public bool IsSellable { get; set; }

        public AddressViewModel Address { get; set; }
    }
}
