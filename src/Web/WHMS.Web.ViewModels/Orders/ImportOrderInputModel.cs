namespace WHMS.Web.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ImportOrderInputModel
    {
        public AddOrderInputModel Order { get; set; }

        public AddOrderItemsInputModel Items { get; set; }
    }
}
