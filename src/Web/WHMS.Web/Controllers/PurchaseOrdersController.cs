namespace WHMS.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
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

        public IActionResult PurchaseOrderDetails(int id)
        {
            var model = this.purchaseOrdersService.GetPurchaseOrderDetails<PurchaseOrderDetailsViewModel>(id);

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

            var userId = this.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            await this.purchaseOrdersService.AddPurchaseOrderAsync(input, userId);
            return this.RedirectToAction(nameof(this.ManagePurchaseOrders));
        }

        public IActionResult AddPurchaseItem(int purchaseOrderId, int vendorId)
        {
            var model = new AddPurchaseItemsInputModel
            {
               PurchaseOrderId = purchaseOrderId,
               VendorId = vendorId,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddPurchaseItem(AddPurchaseItemsInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.purchaseOrdersService.AddPurchaseItemAsync(input);
            return this.RedirectToAction(nameof(this.PurchaseOrderDetails), new { id = input.PurchaseOrderId });
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
        public async Task<IActionResult> VendorDetails(VendorViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.purchaseOrdersService.EditVendorAsync(input);

            var model = this.purchaseOrdersService.GetVendorDetails<VendorViewModel>(input.Id);
            return this.View(model);
        }
    }
}
