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

        public async Task GenerateReports(PerformContext console)
        {
            try
            {
                await GenerateQtySoldReport(console);
                await GenerateQtySoldPerChannelReport(console);
                await GeneateInventorySnapshot(console);
            }
            catch (Exception ex)
            {
                console.WriteLine($"{ DateTime.Now}: {ex.ToString()}");
                throw;
            }

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

        private async Task GenerateQtySoldPerChannelReport(PerformContext console)
        {
            var channels = Enum.GetValues<Channel>();

            foreach (var channel in channels)
            {
                var qtySold = this.context.OrderItems
                    .Where(x => x.Order.CreatedOn >= DateTime.Now.Date && x.Order.Channel == channel)
                    .Sum(x => x.Qty);
                var qtySoldAmount = this.context.OrderItems
                    .Where(x => x.Order.CreatedOn >= DateTime.Now.Date && x.Order.Channel == channel)
                    .Sum(x => x.Qty * x.Price);

                var reportEntry = this.context.QtySoldByChannel.Where(x => x.Date >= DateTime.Now.Date && x.Channel == channel).FirstOrDefault();
                if (reportEntry == null)
                {
                    reportEntry = new QtySoldByChannel();
                    this.context.Add(reportEntry);
                }
                reportEntry.QtySold = qtySold;
                reportEntry.AmountSold = qtySoldAmount;
                reportEntry.Channel = channel;
                reportEntry.Date = DateTime.Now;
                console.WriteLine($"{DateTime.Now}: Channel: {channel.ToString()}, QtySold: {qtySold}, $ amount:  ${qtySoldAmount}");
                await this.context.SaveChangesAsync();
            }
        }

        private async Task GenerateQtySoldReport(PerformContext console)
        {
            var qtySold = this.context.OrderItems.Where(x => x.Order.CreatedOn >= DateTime.Now.Date).Sum(x => x.Qty);
            var qtySoldAmount = this.context.OrderItems.Where(x => x.Order.CreatedOn >= DateTime.Now.Date).Sum(x => x.Qty * x.Price);

            var reportEntry = this.context.QtySoldByDay.Where(x => x.Date >= DateTime.Now.Date).FirstOrDefault();
            if (reportEntry == null)
            {
                reportEntry = new QtySoldByDay();
                this.context.Add(reportEntry);
            }
            reportEntry.QtySold = qtySold;
            reportEntry.AmountSold = qtySoldAmount;
            reportEntry.Date = DateTime.Now;
            console.WriteLine($"{DateTime.Now}: QtySold: {qtySold}, $ amount:  ${qtySoldAmount}");
            await this.context.SaveChangesAsync();
        }
    }
}
