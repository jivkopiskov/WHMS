namespace WHMS.Services.Products
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;
    using WHMS.Data;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;

    public class ConditionsService : ICondiitonsService
    {
        private readonly WHMSDbContext context;
        private readonly IMapper mapper;

        public ConditionsService(WHMSDbContext context)
        {
            this.context = context;
            this.mapper = AutoMapperConfig.MapperInstance;
        }

        public IEnumerable<T> GetAllConditions<T>()
        {
            var conditions = this.context.ProductConditions.To<T>();
            return conditions;
        }

        public async Task<int> CreateProductConditionAsync<T>(T input)
        {
            var condition = this.mapper.Map<ProductCondition>(input);
            this.context.ProductConditions.Add(condition);
            await this.context.SaveChangesAsync();
            return condition.Id;
        }
    }
}
