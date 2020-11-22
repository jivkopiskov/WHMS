namespace WHMS.Services.PurchaseOrders
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using WHMS.Common;
    using WHMS.Data;
    using WHMS.Data.Models.PurchaseOrder;
    using WHMS.Data.Models.PurchaseOrder.Enum;
    using WHMS.Services.Mapping;
    using WHMS.Web.ViewModels.PurchaseOrders;

    public class PurchaseOrdersService : IPurchaseOrdersService
    {
        private readonly WHMSDbContext context;
        private readonly IMapper mapper;

        public PurchaseOrdersService(WHMSDbContext context)
        {
            this.context = context;
            this.mapper = AutoMapperConfig.MapperInstance;
        }

        public async Task AddPurchaseItemAsync(AddPurchaseItemsInputModel input)
        {
            var po = this.context.PurchaseOrders.Include(x => x.PurchaseItems).FirstOrDefault(x => x.Id == input.PurchaseOrderId);
            foreach (var item in input.PurchaseItems)
            {
                var purchaseItem = po.PurchaseItems.FirstOrDefault(x => x.ProductId == item.ProductId);
                if (purchaseItem == null)
                {
                    purchaseItem = new PurchaseItem
                    {
                        ProductId = item.ProductId,
                        Cost = item.Cost,
                        PurchaseOrderId = input.PurchaseOrderId,
                    };
                    this.context.Add(purchaseItem);
                }

                purchaseItem.Qty += item.Qty;
                var vendorProduct = this.context.VendorProducts.FirstOrDefault(x => x.ProductId == item.ProductId && x.VendorId == input.VendorId);
                if (vendorProduct == null)
                {
                    vendorProduct = new VendorProduct
                    {
                        ProductId = item.ProductId,
                        VendorId = po.VendorId,
                    };
                    this.context.Add(vendorProduct);
                }

                vendorProduct.VendorCost = item.Cost;

                await this.context.SaveChangesAsync();

                await this.RecalculatePurchaseOrderTotal(po.Id);
            }
        }

        public async Task<int> AddPurchaseOrderAsync<T>(T input, string createdBy)
        {
            var po = this.mapper.Map<PurchaseOrder>(input);
            po.PurchaseOrderStatus = PurchaseOrderStatus.Created;
            po.ReceivingStatus = ReceivingStatus.Unreceived;
            po.GrandTotal += po.ShippingFee;
            po.CreatedById = createdBy;
            await this.context.AddAsync(po);
            await this.context.SaveChangesAsync();
            return po.Id;
        }

        public async Task<int> AddVendorAsync<T>(T input)
        {
            var vendor = this.mapper.Map<T, Vendor>(input);
            await this.context.AddAsync(vendor);
            await this.context.SaveChangesAsync();
            return vendor.Id;
        }

        public Task<int> DeletePurchaseItemAsync(int purchaseItemId)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> EditPurchaseItemAsync(int purchaseItemId)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> EditPurchaseOrderAsync(int purchaseOrderId)
        {
            throw new System.NotImplementedException();
        }

        public async Task EditVendorAsync(VendorViewModel input)
        {
            var vendor = this.context.Vendors.Include(x => x.Address).FirstOrDefault(x => x.Id == input.Id);
            this.mapper.Map<VendorViewModel, Vendor>(input, vendor);
            await this.context.SaveChangesAsync();
        }

        public int GetAllPurchaseOrdersCount()
        {
            return this.context.PurchaseOrders.Where(x => !x.IsDeleted).Count();
        }

        public int GetAllVendorsCount()
        {
            return this.context.Vendors.Count();
        }

        public IEnumerable<T> GetAllPurchaseOrders<T>(PurchaseOrdersFilterModel input)
        {
            var purchaseOrders = this.context.PurchaseOrders.Where(x => !x.IsDeleted);
            purchaseOrders = this.FilterPurchaseOrders(input, purchaseOrders).OrderByDescending(x => x.Id);
            return purchaseOrders
                .Skip((input.Page - 1) * GlobalConstants.PageSize)
                .Take(GlobalConstants.PageSize)
                .To<T>()
                .ToList();
        }

        public IEnumerable<T> GetAllVendors<T>(int page)
        {
            return this.context.Vendors
                .Skip((page - 1) * GlobalConstants.PageSize)
                .Take(GlobalConstants.PageSize)
                .OrderBy(x => x.Name)
                .To<T>()
                .ToList();
        }

        public IEnumerable<T> GetAllVendors<T>()
        {
            return this.context.Vendors
                .OrderBy(x => x.Name)
                .To<T>()
                .ToList();
        }

        public T GetPurchaseOrderDetails<T>(int purchaseOrderId)
        {
            return this.context.PurchaseOrders
                .Where(x => x.Id == purchaseOrderId)
                .To<T>()
                .FirstOrDefault();
        }

        public T GetVendorDetails<T>(int id)
        {
            var vendor = this.context.Vendors.Where(x => x.Id == id).To<T>().FirstOrDefault();
            return vendor;
        }

        public async Task RecalculatePurchaseOrderTotal(int purchaseOrderId)
        {
            var po = this.context.PurchaseOrders.FirstOrDefault(x => x.Id == purchaseOrderId);
            po.GrandTotal = po.PurchaseItems.Sum(x => x.Qty * x.Cost) + po.ShippingFee;
            await this.context.SaveChangesAsync();
        }

        public Task<int> ReceivePurchaseItem(int purchaseItemId)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> ReceiveWholePurchaseOrder(int purchaseOrderId)
        {
            throw new System.NotImplementedException();
        }

        private IQueryable<PurchaseOrder> FilterPurchaseOrders(PurchaseOrdersFilterModel input, IQueryable<PurchaseOrder> purchaseOrders)
        {
            if (input.Id != null)
            {
                purchaseOrders = purchaseOrders.Where(x => x.Id == input.Id);
            }

            if (input.WarehouseId != null && input.WarehouseId != 0)
            {
                purchaseOrders = purchaseOrders.Where(x => x.WarehouseId == input.WarehouseId);
            }

            if (input.PurchaseOrderStatus != null)
            {
                purchaseOrders = purchaseOrders.Where(x => x.PurchaseOrderStatus == input.PurchaseOrderStatus);
            }

            if (input.ReceivingStatus != null)
            {
                purchaseOrders = purchaseOrders.Where(x => x.ReceivingStatus == input.ReceivingStatus);
            }

            if (input.SKU != null)
            {
                purchaseOrders = purchaseOrders.Where(x => x.PurchaseItems.Any(pi => pi.Product.SKU == input.SKU || pi.ProductId.ToString() == input.SKU));
            }

            if (input.VendorId != null && input.VendorId != 0)
            {
                purchaseOrders = purchaseOrders.Where(x => x.VendorId == input.VendorId);
            }

            return purchaseOrders;
        }
    }
}
