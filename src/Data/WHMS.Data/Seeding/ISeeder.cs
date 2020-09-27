namespace WHMS.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    public interface ISeeder
    {
        Task SeedAsync(WHMSDbContext dbContext, IServiceProvider serviceProvider);
    }
}
