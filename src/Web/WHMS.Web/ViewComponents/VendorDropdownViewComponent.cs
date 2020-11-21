namespace WHMS.Web.ViewComponents
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using WHMS.Services.PurchaseOrders;
    using WHMS.Web.ViewModels.PurchaseOrders;

    public class VendorDropdownViewComponent : ViewComponent
    {
        private readonly IPurchaseOrdersService purchaseOrderService;

        public VendorDropdownViewComponent(IPurchaseOrdersService purchaseOrderService)
        {
            this.purchaseOrderService = purchaseOrderService;
        }

        public IViewComponentResult Invoke(int id)
        {
            var vendors = this.purchaseOrderService.GetAllVendors<VendorViewModel>()
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                    Selected = x.Id == id,
                });
            return this.View(vendors);
        }
    }
}
