namespace WHMS.Services.Tests.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using WHMS.Common;
    using WHMS.Data;
    using WHMS.Data.Models.Products;
    using WHMS.Services.Mapping;
    using WHMS.Services.Products;
    using WHMS.Web.ViewModels;
    using WHMS.Web.ViewModels.Products;
    using Xunit;

    public class ConditionsServiceTests : BaseServiceTest
    {
        [Fact]
        public async Task CreateConditionShouldCreateNewCondition()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            var service = new ConditionsService(context);

            var testCondition = new ConditionViewModel { Name = "Test", Description = "Description" };
            var conditionId = await service.CreateProductConditionAsync(testCondition);
            var condition = context.ProductConditions.FirstOrDefault();

            Assert.Equal(condition.Id, conditionId);
            Assert.Equal(condition.Name, testCondition.Name);
            Assert.Equal(condition.Description, testCondition.Description);
        }

        [Fact]
        public async Task GetAllConditionsShouldReturnAllConditions()
        {
            var options = new DbContextOptionsBuilder<WHMSDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            using var context = new WHMSDbContext(options);
            for (int i = 0; i < 100; i++)
            {
                await context.ProductConditions.AddAsync(new ProductCondition { Name = i.ToString() });
            }

            await context.SaveChangesAsync();
            var service = new ConditionsService(context);

            var conditions = service.GetAllConditions<ConditionViewModel>();
            var conditionsCount = conditions.ToList().Count();
            var exepcetedCount = context.ProductConditions.Count();

            Assert.Equal(exepcetedCount, conditionsCount);
        }
    }
}
