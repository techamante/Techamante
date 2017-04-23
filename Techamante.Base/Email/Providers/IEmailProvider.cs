using System.Threading.Tasks;

namespace Techamante.Email.Providers
{
    public interface IEmailProvider
    {
        Task SendEmailAsync(string fromEmail, string toEmail, string toName, string subject, string bodyHtml);
    }
}