namespace WHMS.Services.Products
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICondiitonsService
    {
        IEnumerable<T> GetAllConditions<T>();

        Task<int> CreateProductConditionAsync<T>(T input);
    }
}
