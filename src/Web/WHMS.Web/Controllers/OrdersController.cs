namespace WHMS.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
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
            await this.ordersService.CreateOrderAsync(input);

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

        public IActionResult CheckCustomerAddress(string email)
        {
            var model = this.ordersService.GetCustomer<CustomerViewModel>(email);
            return this.Json(model);
        }
    }
}
