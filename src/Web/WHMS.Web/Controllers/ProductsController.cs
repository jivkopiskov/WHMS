namespace WHMS.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
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
        public async Task<IActionResult> AddProduct(AddProductInputModel model)
        {
            if (!this.productService.IsSkuAvailable(model.SKU))
            {
                this.TempData["error"] = GlobalConstants.UnavailableSKU;
                return this.View(model);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View();
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
                this.TempData["success"] = false;
                return this.ProductDetails(input.Id);
            }

            ProductDetailsViewModel product = await this.productService.EditProductAsync<ProductDetailsViewModel, ProductDetailsInputModel>(input);
            this.TempData["success"] = true;
            return this.ProductDetails(input.Id);
        }

        public IActionResult ManageInventory(int id)
        {
            var model = this.productService.GetProductWarehouseInfo(id);
            return this.View(model);
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
            // TO DO:
            if (this.ModelState.IsValid)
            {
            }

            await this.productService.UpdateDefaultImageAsync(input.ImageId);

            return this.ProductImages(input.ProductId);
        }

        public IActionResult AddImages(int id)
        {
            var model = new AddImagesInputModel { ProductId = id };
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddImages(AddImagesInputModel input)
        {
            foreach (var image in input.URL)
            {
                if (string.IsNullOrEmpty(image) || !new UrlAttribute().IsValid(image))
                {
                    continue;
                }

                var imageModel = new ImageViewModel() { ProductId = input.ProductId, Url = image };
                await this.productService.AddProductImageAsync(imageModel);
            }

            return this.Redirect("/Products/ProductImages/" + input.ProductId);
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
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

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

        #region conditions
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
        #endregion

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

        public async Task<IActionResult> AdjustInventory(ProductAdjustmentInputModel input)
        {
            if (this.productService.IsValidProductId(input.ProductId))
            {
                await this.productService.AdjustInventory(input);
            }

            return this.Redirect("ManageInventory/" + input.ProductId);
        }
    }
}
