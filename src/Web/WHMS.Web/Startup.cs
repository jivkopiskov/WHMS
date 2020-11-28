namespace WHMS.Web
{
    using System;
    using System.Reflection;

    using Hangfire;
    using Hangfire.Console;
    using Hangfire.Dashboard;
    using Hangfire.SqlServer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using WHMS.Common;
    using WHMS.Data;
    using WHMS.Data.Models;
    using WHMS.Data.Seeding;
    using WHMS.Services;
    using WHMS.Services.Common;
    using WHMS.Services.CronJobs;
    using WHMS.Services.Data.Common;
    using WHMS.Services.Mapping;
    using WHMS.Services.Messaging;
    using WHMS.Services.Orders;
    using WHMS.Services.Products;
    using WHMS.Services.PurchaseOrders;
    using WHMS.Web.ViewModels;

    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.configuration = configuration;
            this.env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(
                config => config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer().UseRecommendedSerializerSettings().UseSqlServerStorage(
                        this.configuration.GetConnectionString("DefaultConnection"),
                        new SqlServerStorageOptions
                        {
                            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                            QueuePollInterval = TimeSpan.Zero,
                            UseRecommendedIsolationLevel = true,
                            UsePageLocksOnDequeue = true,
                            DisableGlobalLocks = true,
                        }).UseConsole());

            services.AddDbContext<WHMSDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            if (this.env.IsDevelopment())
            {
                services.AddDatabaseDeveloperPageExceptionFilter();
            }

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<WHMSDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddControllersWithViews(
                options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }).AddRazorRuntimeCompilation();
            services.AddRazorPages();

            services.AddSingleton(this.configuration);

            // Application services
            services.AddTransient<IEmailSender, SendGridEmailSender>(x => new SendGridEmailSender(this.configuration["SendGridAPIKey"]));
            services.AddTransient<IManualEmailSender, SendGridEmailSender>(x => new SendGridEmailSender(this.configuration["SendGridAPIKey"]));
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<IBrandsService, BrandsService>();
            services.AddTransient<ICondiitonsService, ConditionsService>();
            services.AddTransient<IInventoryService, InventoryService>();
            services.AddTransient<IManufacturersService, ManufacturersService>();
            services.AddTransient<IWarehouseService, WarehouseService>();
            services.AddTransient<IOrdersService, OrdersService>();
            services.AddTransient<ICountriesService, CountriesService>();
            services.AddTransient<ICustomersService, CustomersService>();
            services.AddTransient<IOrderItemsService, OrderItemsService>();
            services.AddTransient<IShippingService, ShippingService>();
            services.AddTransient<IPurchaseOrdersService, PurchaseOrdersService>();
            services.AddTransient<IReportServices, ReportServices>();
            services.AddTransient<IHtmlToPdfConverter, HtmlToPdfConverter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJobManager)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<WHMSDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
                this.SeedHangfireJobs(recurringJobManager, dbContext);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireServer(new BackgroundJobServerOptions { WorkerCount = 2 });
            app.UseHangfireDashboard();

            app.UseEndpoints(
                    endpoints =>
                        {
                            endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                            endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                            endpoints.MapRazorPages();
                        });
        }

        private void SeedHangfireJobs(IRecurringJobManager recurringJobManager, WHMSDbContext dbContext)
        {
            recurringJobManager.AddOrUpdate<ReportsGenerator>("GenerateReports", x => x.GenerateReports(null, DateTime.Now.Date), "0 23 * * *");
            recurringJobManager.AddOrUpdate<ReportsGenerator>("RegenerateYesterdayReports", x => x.GenerateReports(null, DateTime.Now.Date.AddDays(-1)), "0 03 * * *");
            recurringJobManager.AddOrUpdate<ReportsGenerator>("RecalculateQtySoldToday", x => x.GenerateQtySoldReport(null, DateTime.Now), "*/5 * * * *");
        }

        private class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                var httpContext = context.GetHttpContext();
                return httpContext.User.IsInRole(GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
