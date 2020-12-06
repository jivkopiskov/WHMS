namespace WHMS.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Caching.Memory;
    using WHMS.Common;
    using WHMS.Data.Models;
    using WHMS.Services.Common;
    using WHMS.Services.Orders;
    using WHMS.Web.Infrastructure.ModelBinders;
    using WHMS.Web.ViewModels;
    using WHMS.Web.ViewModels.Orders;
    using WHMS.Web.ViewModels.ValidationAttributes;

    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IOrdersService ordersService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICustomersService customersService;
        private readonly IOrderItemsService orderItemsService;
        private readonly IImportOrderServices importServices;
        private readonly IShippingService shippingService;
        private readonly IHtmlToPdfConverter htmlToPdfConverter;
        private readonly IWebHostEnvironment environment;
        private readonly IMemoryCache cache;

        public OrdersController(
            IOrdersService ordersService,
            UserManager<ApplicationUser> userManager,
            ICustomersService customersService,
            IOrderItemsService orderItemsService,
            IImportOrderServices importServices,
            IShippingService shippingService,
            IHtmlToPdfConverter htmlToPdfConverter,
            IWebHostEnvironment environment,
            IMemoryCache cache)
        {
            this.ordersService = ordersService;
            this.userManager = userManager;
            this.customersService = customersService;
            this.orderItemsService = orderItemsService;
            this.importServices = importServices;
            this.shippingService = shippingService;
            this.htmlToPdfConverter = htmlToPdfConverter;
            this.environment = environment;
            this.cache = cache;
        }

        public IActionResult ManageOrders(OrdersFilterInputModel input)
        {
            var model = new ManageOrdersViewModel()
            {
                Page = input.Page,
                Filters = input,
                Orders = this.ordersService.GetAllOrders<OrdersViewModel>(input),
                PagesCount = (int)Math.Ceiling((double)this.ordersService.GetAllOrdersCount() / GlobalConstants.PageSize),
            };

            return this.View(model);
        }

        public IActionResult AddOrder()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(AddOrderInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            input.CreatedById = this.userManager.GetUserId(this.User);
            var orderId = await this.ordersService.AddOrderAsync(input);

            return this.RedirectToAction(nameof(this.OrderDetails), new { id = orderId });
        }

        public IActionResult AddOrderItems(int id)
        {
            var model = new AddOrderItemsInputModel() { OrderId = id };
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderItems([ModelBinder(BinderType = typeof(OrderItemsBinder))] AddOrderItemsInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.orderItemsService.AddOrderItemAsync(input);
            return this.RedirectToAction(nameof(this.OrderDetails), new { id = input.OrderId });
        }

        public async Task<IActionResult> DeleteOrderItem([ValidOrder] int id, int orderItemId)
        {
            if (this.ModelState.IsValid)
            {
                await this.orderItemsService.DeleteOrderItemAsync(orderItemId);
            }
            else
            {
                this.TempData["invalidOrder"] = true;
            }

            return this.RedirectToAction(nameof(this.OrderDetails), new { id });
        }

        public IActionResult ImportOrders()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportOrders(ImportFileInputModel input)
        {
            var file = input.File;
            var folderName = GlobalConstants.UploadedExcelFilesFolder;
            var webRootPath = this.environment.WebRootPath;
            var newPath = Path.Combine(webRootPath, folderName);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }

            if (file == null || file.Length == 0)
            {
                this.ModelState.AddModelError("file", "Invalid file");
                return this.View();
            }

            string uploadFileExtension = Path.GetExtension(file.FileName).ToLower();
            if (uploadFileExtension.ToUpper() != ".XLS" && uploadFileExtension.ToUpper() != ".XLSX")
            {
                this.ModelState.AddModelError("file", "Invalid file format");
                return this.View();
            }

            string fullPath = Path.Combine(newPath, Guid.NewGuid().ToString() + uploadFileExtension);
            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            var errors = await this.importServices.ImportOrdersAsync(stream);
            if (!string.IsNullOrEmpty(errors))
            {
                this.ModelState.AddModelError("file", errors);
                return this.View();
            }

            return this.RedirectToAction(nameof(this.ManageOrders));
        }

        public async Task<IActionResult> DownloadTemplate()
        {
            var result = await System.IO.File.ReadAllBytesAsync(Path.Combine(this.environment.WebRootPath, GlobalConstants.UploadedExcelFilesFolder, GlobalConstants.OrderImportTemplateFileName));
            return this.File(result, "application/vnd.ms-excel");
        }

        public IActionResult OrderDetails(int id)
        {
            var model = this.ordersService.GetOrderDetails<OrderDetailsViewModel>(id);

            return this.View(model);
        }

        public IActionResult AddPayment(int id)
        {
            var model = new PaymentInputModel() { OrderId = id };
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddPayment(PaymentInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.ordersService.AddPaymentAsync(input);

            return this.RedirectToAction(nameof(this.OrderDetails), new { id = input.OrderId });
        }

        public IActionResult ManagePayments(int id)
        {
            var model = new ManagePaymentsViewModel() { OrderId = id };
            model.OrderGrandTotal = this.ordersService.GetOrderDetails<OrderDetailsViewModel>(id).GrandTotal;
            model.Payments = this.ordersService.GetAllPayments<PaymentViewModel>(id);
            return this.View(model);
        }

        public async Task<IActionResult> DeletePayment(int id, int orderId)
        {
            await this.ordersService.DeletePaymentAsync(id);
            return this.RedirectToAction(nameof(this.ManagePayments), new { id = orderId });
        }

        public async Task<IActionResult> CancelOrder([ValidOrder] int id)
        {
            if (this.ModelState.IsValid)
            {
                await this.ordersService.CancelOrderAsync(id);
                this.TempData["CancelOrder"] = true;
            }
            else
            {
                this.TempData["CancelOrder"] = false;
            }

            return this.RedirectToAction(nameof(this.OrderDetails), new { id });
        }

        public async Task<IActionResult> SetInProcess([ValidOrder] int id)
        {
            if (this.ModelState.IsValid)
            {
                await this.ordersService.SetInProcessAsync(id);
                this.TempData["InProcess"] = true;
            }
            else
            {
                this.TempData["InProcess"] = false;
            }

            return this.RedirectToAction(nameof(this.OrderDetails), new { id });
        }

        public IActionResult ShipOrder([ValidOrder] int id)
        {
            var model = new ShipOrderInputModel() { OrderId = id, Carriers = this.shippingService.GetAllCarriers<CarrierViewModel>() };
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ShipOrder(ShipOrderInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var model = new ShipOrderInputModel() { OrderId = input.OrderId, Carriers = this.shippingService.GetAllCarriers<CarrierViewModel>() };
                return this.View(model);
            }

            await this.shippingService.ShipOrderAsync(input);

            return this.RedirectToAction(nameof(this.OrderDetails), new { id = input.OrderId });
        }

        public async Task<IActionResult> UnshipOrder(int id)
        {
            await this.shippingService.UnshipOrderAsync(id);

            return this.RedirectToAction(nameof(this.OrderDetails), new { id });
        }

        public IActionResult ManageCarriers()
        {
            var model = this.shippingService.GetAllCarriers<CarrierViewModel>();
            return this.View(model);
        }

        public IActionResult AddCarrier()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCarrier(CarrierViewModel input)
        {
            await this.shippingService.AddCarrierAsync(input.Name);
            return this.RedirectToAction(nameof(this.ManageCarriers));
        }

        public IActionResult ManageShippingMethods(int id)
        {
            var model = new ManageShippingMethodsViewModel
            {
                Methods = this.shippingService.GetAllServicesForCarrier<ShippingMethodViewModel>(id),
                CarrierName = this.shippingService.GetAllCarriers<CarrierViewModel>().FirstOrDefault(x => x.Id == id).Name,
                CarrierId = id,
            };

            return this.View(model);
        }

        public IActionResult AddShippingMethod(int id)
        {
            var model = new ShippingMethodViewModel { CarrierId = id };
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddShippingMethod(ShippingMethodViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.shippingService.AddShippingMethodAsync(input.CarrierId, input.Name);
            return this.RedirectToAction(nameof(this.ManageShippingMethods), new { id = input.CarrierId });
        }

        public async Task<IActionResult> DeleteShippingMethod(int id, int carrierId)
        {
            await this.shippingService.DeleteShippingMethodAsync(id);

            return this.RedirectToAction(nameof(this.ManageShippingMethods), new { id = carrierId });
        }

        public IActionResult ManageCustomers(CustomersFilterInputModel input)
        {
            var model = new ManageCustomersViewModel()
            {
                Page = input.Page,
                Customers = this.customersService.GetAllCustomers<CustomerViewModel>(input),
                PagesCount = (int)Math.Ceiling((double)this.customersService.CustomersCount() / GlobalConstants.PageSize),
                Filters = input,
            };

            return this.View(model);
        }

        public async Task<IActionResult> GetPdf(int id)
        {
            byte[] result = this.cache.Get<byte[]>("pdf" + id);
            if (result != null)
            {
                return this.File(result, "application/pdf");
            }

            var model = this.ordersService.GetOrderDetails<OrderDetailsViewModel>(id);
            var viewHtml = await this.RenderViewAsync("GetPdf", model);
            result = this.htmlToPdfConverter.Convert(this.environment.ContentRootPath, viewHtml);
            this.cache.Set<byte[]>("pdf" + id, result, new TimeSpan(24, 0, 0));

            return this.File(result, "application/pdf");
        }

        public JsonResult GetMethodsForCarrier(int carrierId)
        {
            var methods = this.shippingService.GetAllServicesForCarrier<ShippingMethodViewModel>(carrierId);
            var result = new SelectList(methods, "Id", "Name");
            return this.Json(result);
        }

        public IActionResult CheckCustomerAddress(string email)
        {
            var model = this.customersService.GetCustomer<CustomerViewModel>(email);
            return this.Json(model);
        }
    }
}
