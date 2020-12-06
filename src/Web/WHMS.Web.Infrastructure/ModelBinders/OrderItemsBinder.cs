namespace WHMS.Web.Infrastructure.ModelBinders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.ModelBinding;

    using WHMS.Web.ViewModels.Orders;

    public class OrderItemsBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            try
            {
                var model = new AddOrderItemsInputModel();
                model.OrderId = int.Parse(bindingContext.HttpContext.Request.Form["orderId"].FirstOrDefault() ?? "0");
                var orderItems = bindingContext.HttpContext.Request.Form["productId"];
                var orderItemsQty = bindingContext.HttpContext.Request.Form["Qty"];
                model.OrderItems = orderItems.Zip(orderItemsQty, (item, qty) => new AddOrderItemViewModel() { ProductId = int.Parse(item), Qty = int.Parse(qty) }).ToList();
                bindingContext.Result = ModelBindingResult.Success(model);
                return Task.CompletedTask;
            }
            catch (Exception)
            {
            }

            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }
    }
}
