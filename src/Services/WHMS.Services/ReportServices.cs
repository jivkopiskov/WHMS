namespace WHMS.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;
    using WHMS.Data;
    using WHMS.Data.Models.Reports;
    using WHMS.Services.Mapping;
    using WHMS.Web.ViewModels.Reports;

    public class ReportServices : IReportServices
    {
        private readonly WHMSDbContext context;
        private readonly IMapper mapper;

        public ReportServices(WHMSDbContext context)
        {
            this.context = context;
            this.mapper = AutoMapperConfig.MapperInstance;
        }

        public QtySoldLastViewModel GetQtySoldLast(int numberOfDays)
        {
            var qtySoldInfo = this.context.QtySoldByDay.Where(x => x.Date.Date >= DateTime.Now.Date.AddDays(-numberOfDays)).To<QtySoldViewModel>().ToList();
            return new QtySoldLastViewModel { QtySoldList = qtySoldInfo };
        }

        public QtySoldViewModel GetQtySoldToday()
        {
            return this.mapper.Map<QtySoldByDay, QtySoldViewModel>(this.context.QtySoldByDay.FirstOrDefault(x => x.Date.Date == DateTime.Now.Date));
        }
    }
}
