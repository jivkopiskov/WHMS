namespace WHMS.Web.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using WHMS.Data.Models.Orders.Enum;

    public class OrdersFilterInputModel : IFilter
    {
        public int Page { get; set; } = 1;

        public string SKU { get; set; }

        [Display(Name = "Customer Email")]
        public string CustomerEmail { get; set; }

        [Display(Name = "Shipping Status")]
        public ShippingStatus? ShippingStatus { get; set; }

        [Display(Name = "Payment Status")]
        public PaymentStatus? PaymentStatus { get; set; }

        [Display(Name = "Payment Status")]
        public OrderStatus? OrderStatus { get; set; }

        public int? WarehouseId { get; set; }

        public Dictionary<string, string> ToDictionary()
        {
            var dict = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(this.SKU))
            {
                dict[nameof(this.SKU)] = this.SKU;
            }

            if (!string.IsNullOrEmpty(this.CustomerEmail))
            {
                dict[nameof(this.CustomerEmail)] = this.CustomerEmail;
            }

            if (this.WarehouseId != null && this.WarehouseId != 0)
            {
                dict["warehouseId"] = this.WarehouseId.ToString();
            }

            if (this.ShippingStatus != null)
            {
                dict[nameof(this.ShippingStatus)] = ((int)this.ShippingStatus).ToString();
            }

            if (this.PaymentStatus != null)
            {
                dict[nameof(this.PaymentStatus)] = ((int)this.PaymentStatus).ToString();
            }

            if (this.OrderStatus != null)
            {
                dict[nameof(this.OrderStatus)] = ((int)this.OrderStatus).ToString();
            }

            return dict;
        }
    }
}
