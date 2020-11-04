namespace WHMS.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using WHMS.Common;
    using WHMS.Data.Models;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Products;
    using WHMS.Web.ViewModels.Products;

    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductsService productService;
        private readonly UserManager<ApplicationUser> userManager;

        public ProductsController(IProductsService productsService, UserManager<ApplicationUser> userManager)
        {
            this.productService = productsService;
            this.userManager = userManager;
        }

        #region products
        public IActionResult ManageProducts(FilterInputModel input)
        {
            var model = new ManageProductsViewModel()
            {
                Page = input.Page,
                Products = this.productService.GetAllProducts<ProductsViewModel>(input),
                PagesCount = (int)Math.Ceiling(this.productService.GetAllProductsCount(input) / (double)GlobalConstants.PageSize),
                Filters = input,
            };

            return this.View(model);
        }

        public IActionResult AddProduct()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductViewModel model)
        {
            if (!this.productService.IsSkuAvailable(model.SKU))
            {
                this.TempData["error"] = GlobalConstants.UnavailableSKU;
                return this.View(model);
            }

            model.CreatedById = this.userManager.GetUserId(this.User);
            await this.productService.CreateProductAsync(model);
            return this.Redirect("/Products/ManageProducts");
        }

        public IActionResult ProductDetails(int id)
        {
            var product = this.productService.GetProductDetails<ProductDetailsViewModel>(id);
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

        public IActionResult ProductImages(int id)
        {
            var model = new ProductImagesViewModel()
            {
                ProductId = id,
                Images = this.productService.GetProductImages<ImageViewModel>(id) ?? new List<ImageViewModel>(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProductImages(ImageViewModel input)
        {
            await this.productService.UpdateDefaultImageAsync(input.Id);
            return this.ProductImages(input.ProductId);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductImage(ImageViewModel input)
        {
            if (this.ModelState.IsValid)
            {
                await this.productService.AddProductImageAsync(input);
                this.TempData["wasSuccess"] = true;
            }
            else
            {
                this.TempData["wasSuccess"] = false;
            }

            return this.Redirect("ProductImages/" + input.ProductId);
        }
        #endregion

        #region brands
        public IActionResult ManageBrands(int page = 1)
        {
            var model = new ManageBrandsViewModel
            {
                Brands = this.productService.GetAllBrands<BrandViewModel>(page),
                Page = page,
                PagesCount = (int)Math.Ceiling(this.productService.GetAllBrandsCount() / (double)GlobalConstants.PageSize),
            };

            return this.View(model);
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
        public IActionResult ManageManufacturers(int page = 1)
        {
            var model = new ManageManufacturersViewModel()
            {
                Manufacturers = this.productService.GetAllManufacturers<ManufacturerViewModel>(page),
                Page = page,
                PagesCount = (int)Math.Ceiling(this.productService.GetAllManufacturersCount() / (double)GlobalConstants.PageSize),
            };

            return this.View(model);
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

        public IActionResult ManageConditions()
        {
            var conditions = this.productService.GetAllConditions<ConditionViewModel>();
            return this.View(conditions);
        }

        public IActionResult AddCondition()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCondition(ConditionViewModel input)
        {
            if (this.ModelState.IsValid)
            {
                await this.productService.AddProductConditionAsync(input);
            }

            return this.Redirect("/Products/ManageConditions");
        }

        public IActionResult ManageWarehouses()
        {
            var warehouses = this.productService.GetAllWarehouses<WarehouseViewModel>();
            return this.View(warehouses);
        }

        public IActionResult AddWarehouse()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddWarehouse(WarehouseViewModel input)
        {
            if (this.ModelState.IsValid)
            {
                await this.productService.CreateWarehouseAsync(input);
            }

            return this.Redirect("/Products/ManageWarehouses");
        }
    }
}
