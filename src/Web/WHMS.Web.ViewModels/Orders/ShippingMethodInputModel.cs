namespace WHMS.Web.ViewModels.Orders
{
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Models.Orders;
    using WHMS.Services.Mapping;

    public class ShippingMethodInputModel
    {
        [Display(Name = "Shipping method")]
        public int Id { get; set; }

        [Display(Name = "Carrier")]
        public int CarrierId { get; set; }
    }
}
