namespace WHMS.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    using WHMS.Common;
    using WHMS.Data.Models;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Messaging;
    using WHMS.Services.Orders;
    using WHMS.Services.Products;
    using WHMS.Web.ViewModels.Orders;
    using WHMS.Web.ViewModels.Products;

    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductsService productService;
        private readonly IBrandsService brandsService;
        private readonly ICondiitonsService condiitonsService;
        private readonly IInventoryService inventoryService;
        private readonly IManufacturersService manufacturersService;
        private readonly IWarehouseService warehouseService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOrderItemsService orderItemsService;
        private readonly IManualEmailSender emailSender;
        private readonly IWebHostEnvironment environment;

        public ProductsController(
            IProductsService productsService,
            IBrandsService brandsService,
            ICondiitonsService condiitonsService,
            IInventoryService inventoryService,
            IManufacturersService manufacturersService,
            IWarehouseService warehouseService,
            UserManager<ApplicationUser> userManager,
            IOrderItemsService orderItemsService,
            IManualEmailSender emailSender,
            IWebHostEnvironment environment)
        {
            this.productService = productsService;
            this.brandsService = brandsService;
            this.condiitonsService = condiitonsService;
            this.inventoryService = inventoryService;
            this.manufacturersService = manufacturersService;
            this.warehouseService = warehouseService;
            this.userManager = userManager;
            this.orderItemsService = orderItemsService;
            this.emailSender = emailSender;
            this.environment = environment;
        }

        public async Task<IActionResult> Test()
        {
            await this.emailSender.SendEmailAsync("jivkopiskov@gmail.com", "Jivko", "jivkopiskov@gmail.com", "Test 1", "Success!");

            return this.StatusCode(1);
        }

        public IActionResult ManageProducts(ProductFilterInputModel input)
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
                this.ModelState.AddModelError("SKU", GlobalConstants.UnavailableSKU);
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

        public IActionResult ImportProducts()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportProducts(ImportProductsInputModel input)
        {
            var file = input.File;
            var folderName = GlobalConstants.UploadedExcelFilesFolder;
            var webRootPath = this.environment.WebRootPath;
            var newPath = Path.Combine(webRootPath, folderName);
            var sb = new StringBuilder();
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }

            if (file == null || file.Length == 0)
            {
                this.ModelState.AddModelError("file", "Invalid file");
                return this.View();
            }

            string uploadFileExtension = Path.GetExtension(file.FileName).ToLower();
            if (uploadFileExtension.ToUpper() != ".XLS" && uploadFileExtension.ToUpper() != ".XLSX")
            {
                this.ModelState.AddModelError("file", "Invalid file format");
                return this.View();
            }

            string fullPath = Path.Combine(newPath, file.FileName);
            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            var errors = await this.productService.ImportProductsAsync(stream);
            if (!string.IsNullOrEmpty(errors))
            {
                this.ModelState.AddModelError("file", errors);
            }

            return this.View();
        }

        public async Task<IActionResult> DownloadTemplate()
        {
            var result = await System.IO.File.ReadAllBytesAsync(Path.Combine(this.environment.WebRootPath, GlobalConstants.UploadedExcelFilesFolder, GlobalConstants.ProductImportTemplateFileName));
            return this.File(result, "application/vnd.ms-excel");
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

            await this.productService.EditProductAsync<ProductDetailsViewModel, ProductDetailsInputModel>(input);
            this.TempData["success"] = true;
            return this.ProductDetails(input.Id);
        }

        public IActionResult AddProductToOrder(int productId)
        {
            var model = new AddProductToOrderInputModel() { ProductId = productId };
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToOrder(AddProductToOrderInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.orderItemsService.AddOrderItemAsync(input);
            return this.Redirect("/Orders/OrderDetails/" + input.OrderId);
        }

        public IActionResult ManageInventory(int id)
        {
            var model = this.warehouseService.GetProductWarehouseInfo(id);
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

        public IActionResult ManageBrands(int page = 1)
        {
            var model = new ManageBrandsViewModel
            {
                Brands = this.brandsService.GetAllBrands<BrandViewModel>(page),
                Page = page,
                PagesCount = (int)Math.Ceiling(this.brandsService.GetAllBrandsCount() / (double)GlobalConstants.PageSize),
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

            await this.brandsService.CreateBrandAsync(name);

            return this.Redirect("/Products/ManageBrands");
        }

        public IActionResult ManageManufacturers(int page = 1)
        {
            var model = new ManageManufacturersViewModel()
            {
                Manufacturers = this.manufacturersService.GetAllManufacturers<ManufacturerViewModel>(page),
                Page = page,
                PagesCount = (int)Math.Ceiling(this.manufacturersService.GetAllManufacturersCount() / (double)GlobalConstants.PageSize),
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
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.manufacturersService.CreateManufacturerAsync(name);

            return this.Redirect("/Products/ManageManufacturers");
        }

        public IActionResult ManageConditions()
        {
            var conditions = this.condiitonsService.GetAllConditions<ConditionViewModel>();
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
                await this.condiitonsService.AddProductConditionAsync(input);
            }

            return this.Redirect("/Products/ManageConditions");
        }

        public IActionResult ManageWarehouses()
        {
            var warehouses = this.warehouseService.GetAllWarehouses<WarehouseViewModel>();
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
                await this.warehouseService.CreateWarehouseAsync(input);
            }

            return this.Redirect("/Products/ManageWarehouses");
        }

        public async Task<IActionResult> AdjustInventory(ProductAdjustmentInputModel input)
        {
            if (this.productService.IsValidProductId(input.ProductId))
            {
                await this.inventoryService.AdjustInventoryAsync(input);
            }

            return this.Redirect("ManageInventory/" + input.ProductId);
        }
    }
}
