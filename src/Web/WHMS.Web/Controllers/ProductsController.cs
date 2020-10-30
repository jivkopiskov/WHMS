﻿namespace WHMS.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using WHMS.Common;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Products;
    using WHMS.Web.ViewModels.Products;

    public class ProductsController : Controller
    {
        private IProductsService productService;

        public ProductsController(IProductsService productsService)
        {
            this.productService = productsService;
        }

        #region products
        public IActionResult ManageProducts(int lastId = 0)
        {
            var products = this.productService.GetAllProducts<ManageProductsViewModel>(lastId);
            return this.View(products);
        }

        public IActionResult AddProduct()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductViewModel model)
        {
            await this.productService.CreateProductAsync(model);

            return this.Redirect("/Products/ManageProducts");
        }

        public IActionResult ProductDetails(int id)
        {
            var product = this.productService.GetProductDetails<ProductDetailsViewModel>(id);
            product.Brands = this.productService.GetAllBrands<BrandViewModel>(0);
            return this.View(product);
        }

        [HttpPost]
        public async Task<IActionResult> ProductDetails(ProductDetailsInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ProductDetails(input.Id);
            }

            ProductDetailsViewModel product = await this.productService.EditProductAsync<ProductDetailsViewModel, ProductDetailsInputModel>(input);        
            return this.ProductDetails(input.Id);
        }
        #endregion

        #region brands
        public IActionResult ManageBrands(int lastId = 0)
        {
            var brands = this.productService.GetAllBrands<BrandViewModel>(lastId);
            return this.View(brands);
        }

        public IActionResult AddBrand()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBrand(string name)
        {
            await this.productService.CreateBrandAsync(name);

            return this.Redirect("/Products/ManageBrands");
        }
        #endregion

        #region manufacturers
        public IActionResult ManageManufacturers(int lastId = 0)
        {
            var brands = this.productService.GetAllManufacturers<ManufacturerViewModel>(lastId);
            return this.View(brands);
        }

        public IActionResult AddManufacturer()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddManufacturer(string name)
        {
            await this.productService.CreateManufacturerAsync(name);

            return this.Redirect("/Products/ManageManufacturers");
        }
        #endregion
    }
}
