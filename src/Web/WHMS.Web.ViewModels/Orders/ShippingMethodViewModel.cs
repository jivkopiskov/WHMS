namespace WHMS.Web.ViewModels.Orders
{
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Models.Orders;
    using WHMS.Services.Mapping;

    public class ShippingMethodViewModel : IMapFrom<ShippingMethod>
    {
        public int Id { get; set; }

        public CarrierViewModel Carrier { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
