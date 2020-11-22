namespace WHMS.Web.ViewModels.PurchaseOrders
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using WHMS.Data.Models.PurchaseOrder;
    using WHMS.Services.Mapping;

    public class PurchaseItemViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ProductSKU { get; set; }

        [Range(1, int.MaxValue)]
        public int Qty { get; set; }

        public decimal Cost { get; set; }

        public string ImageURL { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<PurchaseItem, PurchaseItemViewModel>().ForMember(
                x => x.ImageURL,
                from => from.MapFrom(img => img.Product.Images.Where(i => i.IsPrimary == true).FirstOrDefault().Url));
        }
    }
}
