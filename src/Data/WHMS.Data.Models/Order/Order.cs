namespace WHMS.Data.Models.Order
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Authentication.ExtendedProtection;

    using WHMS.Data.Common.Models;
    using WHMS.Data.Models.Order.Enum;
    using WHMS.Data.Models.Product;

    public class Order : BaseDeletableModel<int>
    {
        public OrderStatus OrderStatus { get; set; }

        [MaxLength(30)]
        public string SourceOrderId { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        public Channel Channel { get; set; }

        public int WarehouseId { get; set; }

        public Warehouse Warehouse { get; set; }

        public DateTime? ShipByDate { get; set; }

        public decimal ShippingFees { get; set; }

        public decimal GrandTotal { get; set; }

        public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();

        public ShippingStatus ShippingStatus { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();

        public int? ShippingMethodId { get; set; }

        public ShippingMethod ShippingMethod { get; set; }

        [MaxLength(50)]
        public string TrackingNumber { get; set; }

        [MaxLength(2048)]
        public string TrackingURL { get; set; }
    }
}
