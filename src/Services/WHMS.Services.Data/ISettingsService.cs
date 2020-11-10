namespace WHMS.Services.Data.Common
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using WHMS.Data.Models;

    public interface ISettingsService
    {
        int GetCount();

        IEnumerable<T> GetAll<T>();

        Task<Setting> AddAsync(Setting setting);

        Task<int> SaveChangesAsync();
    }
}
