namespace WHMS.Services.Products
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBrandsService
    {
        Task<int> CreateBrandAsync(string brandName);

        Task<int> EditBrandAsync(int brandId);

        IEnumerable<T> GetAllBrands<T>();

        IEnumerable<T> GetAllBrands<T>(int page);

        int GetAllBrandsCount();
    }
}
