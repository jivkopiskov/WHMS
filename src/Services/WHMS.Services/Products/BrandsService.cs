namespace WHMS.Services.Products
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using WHMS.Common;
    using WHMS.Data;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;

    public class BrandsService : IBrandsService
    {
        private readonly WHMSDbContext context;
        private readonly IMapper mapper;

        public BrandsService(WHMSDbContext context)
        {
            this.context = context;
            this.mapper = AutoMapperConfig.MapperInstance;
        }

        public async Task<int> CreateBrandAsync(string brandName)
        {
            var brand = new Brand() { Name = brandName };
            await this.context.Brands.AddAsync(brand);
            await this.context.SaveChangesAsync();
            return brand.Id;
        }

        public Task<int> EditBrandAsync(int brandId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> GetAllBrands<T>(int page = 1)
        {
            var brands = this.context.Brands
                .Skip((page - 1) * GlobalConstants.PageSize)
                .Take(GlobalConstants.PageSize)
                .To<T>()
                .ToList();
            return brands;
        }

        public IEnumerable<T> GetAllBrands<T>()
        {
            var brands = this.context.Brands
                .To<T>()
                .ToList();
            return brands;
        }

        public int GetAllBrandsCount()
        {
            return this.context.Brands.Count();
        }
    }
}
