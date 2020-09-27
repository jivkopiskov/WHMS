namespace WHMS.Services.Data
{
    using System.Collections.Generic;

    using WHMS.Data.Models;

    public interface ISettingsService
    {
        int GetCount();

        IEnumerable<Setting> GetAll();
    }
}
