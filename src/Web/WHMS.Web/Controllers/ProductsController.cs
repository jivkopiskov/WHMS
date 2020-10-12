namespace WHMS.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using WHMS.Services.Products;

    public class ProductsController : Controller
    {
        private IProductsService productService;

        public ProductsController(IProductsService productsService)
        {
            this.productService = productsService;
        }

        public IActionResult ManageProducts(int lastId = 0)
        {
            var model = this.productService.GetAllProducts(lastId);
            return this.View(model);
        }
    }
}
