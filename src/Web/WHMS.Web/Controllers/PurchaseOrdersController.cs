namespace WHMS.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using WHMS.Common;
    using WHMS.Services.PurchaseOrders;
    using WHMS.Web.ViewModels.PurchaseOrders;

    public class PurchaseOrdersController : Controller
    {
        private readonly IPurchaseOrdersService purchaseOrdersService;

        public PurchaseOrdersController(IPurchaseOrdersService purchaseOrdersService)
        {
            this.purchaseOrdersService = purchaseOrdersService;
        }

        public IActionResult ManagePurchaseOrders(PurchaseOrdersFilterModel input)
        {
            var model = new ManagePurchaseOrdersViewModel
            {
                Filters = input,
                Page = input.Page,
                PurchaseOrders = this.purchaseOrdersService.GetAllPurchaseOrders<PurchaseOrderSummaryViewModel>(input),
                PagesCount = (int)Math.Ceiling(this.purchaseOrdersService.GetAllPurchaseOrdersCount() / (double)GlobalConstants.PageSize),
            };

            return this.View(model);
        }

        public IActionResult AddPurchaseOrder()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPurchaseOrder(AddPurchaseOrderInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.purchaseOrdersService.AddPurchaseOrderAsync(input);
            return this.RedirectToAction(nameof(this.ManagePurchaseOrders));
        }

        public IActionResult ManageVendors(int page = 1)
        {
            var model = new ManageVendorsViewModel
            {
                Page = page,
                Vendors = this.purchaseOrdersService.GetAllVendors<AddVendorViewModel>(page),
                PagesCount = (int)Math.Ceiling(this.purchaseOrdersService.GetAllVendorsCount() / (double)GlobalConstants.PageSize),
            };
            return this.View(model);
        }

        public IActionResult AddVendor()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddVendor(AddVendorViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.purchaseOrdersService.AddVendorAsync(input);
            return this.RedirectToAction(nameof(this.ManageVendors));
        }

        public IActionResult VendorDetails(int id)
        {
            var model = this.purchaseOrdersService.GetVendorDetails<VendorViewModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public IActionResult VendorDetails(VendorViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            return this.View(input);
        }
    }
}
