namespace WHMS.Web.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using AutoMapper;
    using WHMS.Data.Models.Orders;
    using WHMS.Data.Models.Orders.Enum;
    using WHMS.Services.Mapping;

    public class OrdersViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }

        public Channel Channel { get; set; }

        public string OrderSourceOrderNumber { get; set; }

        public string CustomerName { get; set; }

        public decimal GrandTotal { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public ShippingStatus ShippingStatus { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, OrdersViewModel>().ForMember(
                x => x.CustomerName,
                opt => opt.MapFrom(c => $"{c.Customer.FirstName} {c.Customer.LastName}"));
        }
    }
}
