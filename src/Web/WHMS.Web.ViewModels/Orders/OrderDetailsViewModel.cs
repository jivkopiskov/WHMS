﻿namespace WHMS.Web.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;

    using AutoMapper;
    using WHMS.Data.Models.Orders;
    using WHMS.Data.Models.Orders.Enum;
    using WHMS.Services.Mapping;

    public class OrderDetailsViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }

        public Channel Channel { get; set; }

        public string SourceOrderId { get; set; }

        public CustomerViewModel Customer { get; set; }

        public decimal GrandTotal { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public ShippingStatus ShippingStatus { get; set; }

        public IEnumerable<OrderItemViewModel> OrderItems { get; set; }

        public string WarehouseName { get; set; }

        public DateTime? ShipByDate { get; set; }

        public ShippingMethodViewModel ShippingMethod { get; set; }

        public string TrackingNumber { get; set; }

        public string CreatedByEmail { get; set; }

        public decimal PaidAmount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, OrderDetailsViewModel>().ForMember(
                dest => dest.PaidAmount,
                src => src.MapFrom(o => o.Payments.Sum(x => x.Amount)));
        }
    }
}
