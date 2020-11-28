namespace WHMS.Services.CronJobs
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Hangfire.Console;
    using Hangfire.Server;
    using WHMS.Data;
    using WHMS.Data.Models.Orders.Enum;
    using WHMS.Data.Models.Reports;


    public class ReportsGenerator
    {
        private readonly WHMSDbContext context;

        public ReportsGenerator(WHMSDbContext context)
        {
            this.context = context;
        }

        public async Task GenerateReports(PerformContext console, DateTime date)
        {
            try
            {
                await GenerateQtySoldReport(console, date);
                await GenerateQtySoldPerChannelReport(console, date);
                await GeneateInventorySnapshot(console);
            }
            catch (Exception ex)
            {
                console.WriteLine($"{ DateTime.Now}: {ex.ToString()}");
                throw;
            }

        }
        public async Task GenerateQtySoldReport(PerformContext console, DateTime date)
        {
            var qtySold = this.context.OrderItems.Where(x => x.Order.CreatedOn >= date.Date && x.Order.CreatedOn < date.Date.AddDays(1)).Sum(x => x.Qty);
            var qtySoldAmount = this.context.OrderItems.Where(x => x.Order.CreatedOn >= date.Date && x.Order.CreatedOn < date.Date.AddDays(1)).Sum(x => x.Qty * x.Price);

            var reportEntry = this.context.QtySoldByDay.Where(x => x.Date >= date.Date && x.Date < date.Date.AddDays(1)).FirstOrDefault();
            if (reportEntry == null)
            {
                reportEntry = new QtySoldByDay();
                this.context.Add(reportEntry);
            }
            reportEntry.QtySold = qtySold;
            reportEntry.AmountSold = qtySoldAmount;
            reportEntry.Date = date;
            console.WriteLine($"{DateTime.Now}: QtySold: {qtySold}, $ amount:  ${qtySoldAmount}");
            await this.context.SaveChangesAsync();
        }

        private async Task GeneateInventorySnapshot(PerformContext console)
        {
            var productInventoryInfo = this.context.ProductWarehouses
                .OrderBy(x => x.ProductId)
                .Select(x => new InventorySnapshot
                {
                    Date = DateTime.Now,
                    ProductId = x.ProductId,
                    AggregateQuantity = x.AggregateQuantity,
                    TotalPhysicalQuanitiy = x.TotalPhysicalQuanitiy,
                    ReservedQuantity = x.ReservedQuantity,
                })
                .ToList();

            await this.context.AddRangeAsync(productInventoryInfo);
            await this.context.SaveChangesAsync();
        }

        private async Task GenerateQtySoldPerChannelReport(PerformContext console, DateTime date)
        {
            var channels = Enum.GetValues<Channel>();

            foreach (var channel in channels)
            {
                var qtySold = this.context.OrderItems
                    .Where(x => x.Order.CreatedOn >= date.Date && x.Order.CreatedOn < date.Date.AddDays(1) && x.Order.Channel == channel)
                    .Sum(x => x.Qty);
                var qtySoldAmount = this.context.OrderItems
                    .Where(x => x.Order.CreatedOn >= date.Date && x.Order.CreatedOn < date.Date.AddDays(1) && x.Order.Channel == channel)
                    .Sum(x => x.Qty * x.Price);

                var reportEntry = this.context.QtySoldByChannel.Where(x => x.Date >= date.Date && x.Date < date.Date.AddDays(1) && x.Channel == channel).FirstOrDefault();
                if (reportEntry == null)
                {
                    reportEntry = new QtySoldByChannel();
                    this.context.Add(reportEntry);
                }
                reportEntry.QtySold = qtySold;
                reportEntry.AmountSold = qtySoldAmount;
                reportEntry.Channel = channel;
                reportEntry.Date = date;
                console.WriteLine($"{DateTime.Now}: Channel: {channel.ToString()}, QtySold: {qtySold}, $ amount:  ${qtySoldAmount}");
                await this.context.SaveChangesAsync();
            }
        }
    }
}
