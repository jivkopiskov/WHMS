namespace WHMS.Data.Models.Order
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Authentication.ExtendedProtection;

    using WHMS.Data.Common.Models;
    using WHMS.Data.Models.Order.Enum;

    public class Order : BaseDeletableModel<int>
    {
        public OrderStatus OrderStatus { get; set; }

        public string SourceOrderId { get; set; }

        public Customer Customer { get; set; }

        public Channel Channel { get; set; }

        public DateTime? ShipByDate { get; set; }

        public decimal ShippingFees { get; set; }

        public decimal GrandTotal { get; set; }

        public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();

        public ShippingStatus ShippingStatus { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }
}
