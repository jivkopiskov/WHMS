namespace WHMS.Web.ViewModels.Orders
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using WHMS.Data.Models.Orders;
    using WHMS.Services.Mapping;

    public class OrderItemViewModel : IHaveCustomMappings
    {
        public int ProductId { get; set; }

        public string ProductSKU { get; set; }

        [Range(1, int.MaxValue)]
        public int Qty { get; set; }

        public decimal Price { get; set; }

        public string ImageURL { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<OrderItem, OrderItemViewModel>().ForMember(
                x => x.ImageURL,
                from => from.MapFrom(img => img.Product.Images.Where(i => i.IsPrimary == true).FirstOrDefault().Url));
        }
    }
}
