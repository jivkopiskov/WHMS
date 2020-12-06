namespace WHMS.Services.Orders
{
    using System.IO;
    using System.Threading.Tasks;

    public interface IImportOrderServices
    {
        Task<string> ImportOrdersAsync(Stream stream);
    }
}
