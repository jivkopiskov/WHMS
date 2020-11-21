namespace WHMS.Services.PurchaseOrders
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using WHMS.Common;
    using WHMS.Data;
    using WHMS.Data.Models.PurchaseOrders;
    using WHMS.Services.Mapping;

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

        public Task<int> CreatePurchaseOrderAsync()
        {
            throw new System.NotImplementedException();
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

        public int GetAllVendorsCount()
        {
            return this.context.Vendors.Count();
        }

        public Task<int> GetAllPurchaseOrders(int purchaseOrderId)
        {
            throw new System.NotImplementedException();
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
    }
}
