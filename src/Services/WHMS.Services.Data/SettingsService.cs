namespace WHMS.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

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

        IEnumerable<Setting> ISettingsService.GetAll()
        {
            return this.context.Settings.ToList();
        }
    }
}
