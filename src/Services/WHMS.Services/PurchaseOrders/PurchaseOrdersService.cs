namespace WHMS.Services.PurchaseOrders
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using WHMS.Common;
    using WHMS.Data;
    using WHMS.Data.Models.PurchaseOrders;
    using WHMS.Data.Models.PurchaseOrders.Enum;
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

        public Task<int> AddPurchaseItemAsync(int purchaseOrderId, int productId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> AddPurchaseOrderAsync<T>(T input)
        {
            var po = this.mapper.Map<PurchaseOrder>(input);
            po.PurchaseOrderStatus = PurchaseOrderStatus.Created;
            po.ReceivingStatus = ReceivingStatus.Unreceived;
            po.GrandTotal += po.ShippingFee;
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

        public Task<int> EditVendorAsync(int vendorId)
        {
            throw new System.NotImplementedException();
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

        public Task<int> GetPurchaseOrderDetails(int purchaseOrderId)
        {
            throw new System.NotImplementedException();
        }

        public T GetVendorDetails<T>(int id)
        {
            var vendor = this.context.Vendors.Where(x => x.Id == id).To<T>().FirstOrDefault();
            return vendor;
        }

        public Task<int> RecalculatePurchaseOrderTotal(int purchaseOrderId)
        {
            throw new System.NotImplementedException();
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
