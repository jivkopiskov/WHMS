using WHMS.Web.ViewModels.Reports;

namespace WHMS.Services
{
    public interface IReportServices
    {
        QtySoldLastViewModel GetQtySoldLast(int numberOfDays);

        QtySoldViewModel GetQtySoldToday();
    }
}
