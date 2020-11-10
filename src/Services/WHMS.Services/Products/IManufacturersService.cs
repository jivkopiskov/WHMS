namespace WHMS.Services.Products
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IManufacturersService
    {
        Task<int> CreateManufacturerAsync(string manufactuerName);

        Task<int> EditManufactuerAsync(int manufactuerId);

        IEnumerable<T> GetAllManufacturers<T>(int page);

        IEnumerable<T> GetAllManufacturers<T>();

        int GetAllManufacturersCount();
    }
}
