namespace WHMS.Services
{
    using System.Collections.Generic;

    using WHMS.Web.ViewModels.Reports;

    public interface IReportServices
    {
        QtySoldLastViewModel GetQtySoldLast(int numberOfDays);

        QtySoldViewModel GetQtySoldToday();

        IEnumerable<QtySoldByChannelViewModel> GetQtySoldPerChannelLast(int numberOfDays);
    }
}
