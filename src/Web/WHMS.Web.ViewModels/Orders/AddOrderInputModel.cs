namespace WHMS.Web.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using WHMS.Data.Models.Orders;
    using WHMS.Data.Models.Orders.Enum;
    using WHMS.Services.Mapping;
    using WHMS.Web.ViewModels.Products;

    public class AddOrderInputModel : IMapTo<Order>
    {
        [MaxLength(30)]
        [Display(Name = "Source Order #")]
        public string SourceOrderId { get; set; }

        public int CustomerId { get; set; }

        public CustomerViewModel Customer { get; set; }

        public Channel Channel { get; set; }

        public int WarehouseId { get; set; }

        [Display(Name = "Ship by date")]
        public DateTime? ShipByDate { get; set; }

        public ShippingMethodViewModel ShippingMethod { get; set; }

        [Display(Name = "Salesman")]
        public string CreatedById { get; set; }
    }
}
