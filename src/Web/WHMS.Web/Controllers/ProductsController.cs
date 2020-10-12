namespace WHMS.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using WHMS.Services.Products;
    using WHMS.Web.ViewModels.Products;

    public class ProductsController : Controller
    {
        private IProductsService productService;

        public ProductsController(IProductsService productsService)
        {
            this.productService = productsService;
        }

        public IActionResult ManageProducts(int lastId = 0)
        {
            var products = this.productService.GetAllProducts(lastId);
            return this.View(products);
        }

        public IActionResult AddProduct()
        {
            return this.View();
        }

        [HttpPost("/Products/AddProduct")]
        public async Task<IActionResult> AddProduct(AddProductViewModel model)
        {
            await this.productService.CreateProductAsync(model);

            return this.Redirect("/Products/ManageProducts");
        }

        public IActionResult ManageBrands(int lastId = 0)
        {
            var brands = this.productService.GetAllBrands(lastId);
            return this.View(brands);
        }

        public IActionResult AddBrand()
        {
            return this.View();
        }

        [HttpPost("/Products/AddBrand")]
        public async Task<IActionResult> AddBrand(string name)
        {
            await this.productService.CreateBrandAsync(name);

            return this.Redirect("/Products/ManageBrands");
        }
    }
}
