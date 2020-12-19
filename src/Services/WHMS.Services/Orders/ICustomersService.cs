namespace WHMS.Services.Orders
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WHMS.Data.Models.Orders;
    using WHMS.Web.ViewModels.Orders;

    public interface ICustomersService
    {
        Task<Customer> CreateOrUpdateCustomerAsync(CustomerViewModel input);

        T GetCustomer<T>(string email);

        Task<int> EditCustomerAsync(int customerId);

        Task<int> GetCustomerOrdersAsync(int customerId);

        int CustomersCount();

        IEnumerable<T> GetAllCustomers<T>(CustomersFilterInputModel input);
    }
}
