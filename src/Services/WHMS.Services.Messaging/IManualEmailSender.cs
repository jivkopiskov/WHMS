namespace WHMS.Services.Messaging
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IManualEmailSender
    {
        Task SendEmailAsync(
            string from,
            string fromName,
            string to,
            string subject,
            string htmlContent,
            IEnumerable<EmailAttachment> attachments = null);
    }
}
