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

    public class ManufacturersService : IManufacturersService
    {
        private WHMSDbContext context;
        private IMapper mapper;

        public ManufacturersService(WHMSDbContext context)
        {
            this.context = context;
            this.mapper = AutoMapperConfig.MapperInstance;
        }

        public async Task<int> CreateManufacturerAsync(string manufactuerName)
        {
            var manufacturer = new Manufacturer() { Name = manufactuerName };
            await this.context.Manufacturers.AddAsync(manufacturer);
            await this.context.SaveChangesAsync();
            return manufacturer.Id;
        }

        public Task<int> EditManufactuerAsync(int manufactuerId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> GetAllManufacturers<T>(int page = 1)
        {
            var manufacturers = this.context.Manufacturers
                .Skip((page - 1) * GlobalConstants.PageSize)
                .Take(GlobalConstants.PageSize)
                .To<T>()
                .ToList();
            return manufacturers;
        }

        public IEnumerable<T> GetAllManufacturers<T>()
        {
            var manufacturers = this.context.Manufacturers
                .To<T>()
                .ToList();
            return manufacturers;
        }

        public int GetAllManufacturersCount()
        {
            return this.context.Manufacturers.Count();
        }
    }
}
