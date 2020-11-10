namespace WHMS.Services.Data.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using WHMS.Data;
    using WHMS.Data.Models;
    using WHMS.Services.Mapping;

    public class SettingsService : ISettingsService
    {
        private readonly WHMSDbContext context;

        public SettingsService(WHMSDbContext context)
        {
            this.context = context;
        }

        public int GetCount()
        {
            return this.context.Settings.Count();
        }

        public async Task<Setting> AddAsync(Setting setting)
        {
            var s = await this.context.Settings.AddAsync(setting);
            return s.Entity;
        }

        public Task<int> SaveChangesAsync() => this.context.SaveChangesAsync();

        public IEnumerable<T> GetAll<T>()
        {
           return this.context.Settings.Where(x => 1 == 1).To<T>().ToList();
        }
    }
}
