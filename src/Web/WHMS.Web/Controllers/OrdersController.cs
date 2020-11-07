namespace WHMS.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using WHMS.Services.Orders;
    using WHMS.Web.Infrastructure.ModelBinders;
    using WHMS.Web.ViewModels;
    using WHMS.Web.ViewModels.Orders;

    public class OrdersController : Controller
    {
        private readonly IOrdersService ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
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
            await this.ordersService.CreateOrderAsync(input);

            return this.RedirectToAction(nameof(this.ManageOrders));
        }

        public IActionResult AddOrderItems(int id)
        {
            var model = new AddOrderItemsInputModel() { OrderId = id };
            return this.View(model);
        }

        [HttpPost]
        public IActionResult AddOrderItems([ModelBinder(BinderType = typeof(OrderItemsBinder))] AddOrderItemsInputModel input)
        {
            var model = input;
            return this.View(model);
        }

        public IActionResult CheckCustomerAddress(string email)
        {
            var model = this.ordersService.GetCustomer<CustomerViewModel>(email);
            return this.Json(model);
        }
    }
}
