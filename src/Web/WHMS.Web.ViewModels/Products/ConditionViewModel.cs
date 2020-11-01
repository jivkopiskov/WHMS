namespace WHMS.Web.ViewModels.Products
{
    using System.ComponentModel.DataAnnotations;

    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;

    public class ConditionViewModel : IMapFrom<ProductCondition>, IMapTo<ProductCondition>
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }
    }
}
