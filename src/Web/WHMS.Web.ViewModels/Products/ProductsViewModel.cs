namespace WHMS.Web.ViewModels.Products
{
    using System.Linq;

    using AutoMapper;

    using WHMS.Data.Models.Products;

    using WHMS.Services.Mapping;

    public class ProductsViewModel : IMapFrom<Product>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string SKU { get; set; }

        public string ProductName { get; set; }

        public string ShortDescription { get; set; }

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductsViewModel>().ForMember(
                x => x.ImageUrl,
                opt => opt.MapFrom(x => x.Images.Where(i => i.IsPrimary).FirstOrDefault().Url));
        }
    }
}
