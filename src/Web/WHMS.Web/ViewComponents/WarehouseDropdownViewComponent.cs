namespace WHMS.Web.ViewComponents
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using WHMS.Services.Products;
    using WHMS.Web.ViewModels.Products;

    public class WarehouseDropdownViewComponent : ViewComponent
    {
        private readonly IWarehouseService warehouseService;

        public WarehouseDropdownViewComponent(IWarehouseService warehouseService)
        {
            this.warehouseService = warehouseService;
        }

        public IViewComponentResult Invoke()
        {
            var warehouses = this.warehouseService.GetAllWarehouses<WarehouseViewModel>()
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                });
            return this.View(warehouses);
        }
    }
}
