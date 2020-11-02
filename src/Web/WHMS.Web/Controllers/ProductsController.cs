namespace WHMS.Web.Controllers
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
        public IActionResult ManageProducts(int page = 1)
        {
            var model = new ManageProductsViewModel()
            {
                Page = page,
                Products = this.productService.GetAllProducts<ProductsViewModel>(page),
                PagesCount = (int)Math.Ceiling(this.productService.GetAllProductsCount() / (double)GlobalConstants.PageSize),
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

        public IActionResult ManageConditions(int lastId = 0)
        {
            var conditions = this.productService.GetAllConditions<ConditionViewModel>(lastId);
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
    }
}
