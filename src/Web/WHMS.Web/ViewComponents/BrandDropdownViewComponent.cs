namespace WHMS.Web.ViewComponents
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using WHMS.Services.Products;
    using WHMS.Web.ViewModels.Products;

    public class BrandDropdownViewComponent : ViewComponent
    {
        private readonly IBrandsService brandsService;

        public BrandDropdownViewComponent(IBrandsService brandsService)
        {
            this.brandsService = brandsService;
        }

        public IViewComponentResult Invoke(int id)
        {
            var brands = this.brandsService.GetAllBrands<BrandViewModel>().
                OrderBy(x => x.Name).
                Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                    Selected = x.Id == id,
                });
            return this.View(brands);
        }
    }
}
