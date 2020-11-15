namespace WHMS.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using WHMS.Data.Models;
    using WHMS.Services.Orders;
    using WHMS.Web.Infrastructure.ModelBinders;
    using WHMS.Web.ViewModels;
    using WHMS.Web.ViewModels.Orders;
    using WHMS.Web.ViewModels.ValidationAttributes;

    public class OrdersController : Controller
    {
        private readonly IOrdersService ordersService;
        private readonly UserManager<ApplicationUser> userManager;

        public OrdersController(IOrdersService ordersService, UserManager<ApplicationUser> userManager)
        {
            this.ordersService = ordersService;
            this.userManager = userManager;
        }

        public IActionResult ManageOrders(OrdersFilterInputModel input)
        {
            var model = new ManageOrdersViewModel();
            model.Orders = this.ordersService.GetAllOrders<OrdersViewModel>(input);
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
            await this.ordersService.AddOrderAsync(input);

            return this.RedirectToAction(nameof(this.ManageOrders));
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

            await this.ordersService.AddOrderItemAsync(input);
            return this.View(input);
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

            return this.RedirectToAction(nameof(this.OrderDetails), new { id = id });
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

            return this.RedirectToAction(nameof(this.OrderDetails), new { id = id });
        }

        public IActionResult ShipOrder([ValidOrder] int id)
        {
            var model = new ShipOrderInputModel() { OrderId = id, Carriers = this.ordersService.GetAllCarriers<CarrierViewModel>() };
            return this.View(model);
        }

        [HttpPost]
        public IActionResult ShipOrder(ShipOrderInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            this.ordersService.ShipOrderAsync(input);

            return this.RedirectToAction(nameof(this.OrderDetails), new { id = input.OrderId });
        }

        public JsonResult GetMethodsForCarrier(int carrierId)
        {
            var methods = this.ordersService.GetAllServicesForCarrier<ShippingMethodViewModel>(carrierId);
            var result = new SelectList(methods, "Id", "Name");
            return this.Json(result);
        }

        public IActionResult CheckCustomerAddress(string email)
        {
            var model = this.ordersService.GetCustomer<CustomerViewModel>(email);
            return this.Json(model);
        }
    }
}
