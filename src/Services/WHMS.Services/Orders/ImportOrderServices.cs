namespace WHMS.Services.Orders
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using WHMS.Common;
    using WHMS.Web.ViewModels;
    using WHMS.Web.ViewModels.Orders;

    public class ImportOrderServices : IImportOrderServices
    {
        private readonly IOrdersService orderService;
        private readonly IOrderItemsService orderItemsService;

        public ImportOrderServices(IOrdersService ordersService, IOrderItemsService orderItemsService)
        {
            this.orderService = ordersService;
            this.orderItemsService = orderItemsService;
        }

        public async Task<string> ImportOrdersAsync(Stream stream)
        {
            var sb = new StringBuilder();
            var dt = ExcelHelperClass.GetDataTableFromExcel(stream);

            var orders = this.ConvertDatatableToOrderInputEnumrable(dt);

            foreach (var order in orders)
            {
                ICollection<ValidationResult> validationResults = new List<ValidationResult>();
                if (!ExcelHelperClass.TryValidate(order.Order, out validationResults))
                {
                    sb.AppendLine(string.Join(Environment.NewLine, validationResults.Select(x => x.ErrorMessage)));
                    continue;
                }
                else
                {
                    var orderId = await this.orderService.AddOrderAsync(order.Order);
                    order.Items.OrderId = orderId;
                    try
                    {
                        await this.orderItemsService.AddOrderItemAsync(order.Items);
                    }
                    catch (Exception)
                    {
                        await this.orderService.DeleteOrderAsync(orderId);
                        sb.AppendLine($"Invalid Product ID or qty for order {order.Order.SourceOrderId}. Couldn't add the items.");
                    }
                }
            }

            return sb.ToString();
        }

        private IEnumerable<ImportOrderInputModel> ConvertDatatableToOrderInputEnumrable(DataTable dt)
        {
            var orders = new List<ImportOrderInputModel>();
            foreach (DataRow row in dt.Rows)
            {
                var order = orders.FirstOrDefault(x => x.Order.SourceOrderId == row["SourceOrderId"].ToString());
                if (order == null)
                {
                    order = new ImportOrderInputModel();
                    order.Order = new AddOrderInputModel
                    {
                        SourceOrderId = row["SourceOrderId"].ToString(),
                        Customer = new CustomerViewModel
                        {
                            Email = row["CustomerEmail"].ToString(),
                            FirstName = row["CustomerFirstName"].ToString(),
                            LastName = row["CustomerLastName"].ToString(),
                            PhoneNumber = row["CustomerPhoneNumber"].ToString(),
                            Address = new AddressViewModel
                            {
                                StreetAddress = row["StreetAddress"].ToString(),
                                StreetAddress2 = row["StreetAddress2"].ToString(),
                                City = row["City"].ToString(),
                                Zip = row["Zip"].ToString(),
                                Country = row["Country"].ToString(),
                            },
                        },
                    };
                    order.Items = new AddOrderItemsInputModel();
                    order.Items.OrderItems = new List<AddOrderItemViewModel>();
                    orders.Add(order);
                }

                order.Items.OrderItems.Add(new AddOrderItemViewModel
                {
                    ProductId = int.Parse(row["ProductId"].ToString()),
                    Qty = int.Parse(row["Qty"].ToString()),
                });
            }
            return orders;
        }
    }
}
