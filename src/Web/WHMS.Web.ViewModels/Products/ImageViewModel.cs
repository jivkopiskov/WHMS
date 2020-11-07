namespace WHMS.Web.ViewModels.Products
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;

    public class ImageViewModel : IHaveCustomMappings, IMapTo<Image>
    {
        public int ImageId { get; set; }

        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Url]
        public string Url { get; set; }

        public bool IsPrimary { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Image, ImageViewModel>()
                .ForMember(
                 x => x.ImageId,
                 opt => opt.MapFrom(x => x.Id));
        }
    }
}
