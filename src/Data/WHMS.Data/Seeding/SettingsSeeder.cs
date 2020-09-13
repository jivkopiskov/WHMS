namespace WHMS.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using WHMS.Data.Models;

    internal class SettingsSeeder : ISeeder
    {
        public async Task SeedAsync(WhmsDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Settings.Any())
            {
                return;
            }

            await dbContext.Settings.AddAsync(new Setting { Name = "Setting1", Value = "value1" });
        }
    }
}
