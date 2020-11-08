﻿namespace WHMS.Web.ViewModels.Orders
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Models.Orders;
    using WHMS.Services.Mapping;

    public class CarrierViewModel : IMapFrom<Carrier>
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public ICollection<ShippingMethodViewModel> ShippingMethods { get; set; }
    }
}
