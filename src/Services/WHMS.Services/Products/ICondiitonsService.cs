namespace WHMS.Services.Products
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICondiitonsService
    {
        IEnumerable<T> GetAllConditions<T>(int id = 0);

        Task<int> AddProductConditionAsync<T>(T input);
    }
}
