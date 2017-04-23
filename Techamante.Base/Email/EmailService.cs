using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Techamante.Email.Builders;
using Techamante.Email.Parameters;
using Techamante.Email.Providers;

namespace Techamante.Email
{
    public class EmailService
    {

        private readonly IEmailProvider _emailProvider;
        private readonly string _templatePath;

        public EmailService(IEmailProvider emailProvider, string templatePath)
        {
            _emailProvider = emailProvider;
            _templatePath = templatePath;
        }

        public async Task SendAsyncEMail(BaseEmailParameters parameters, string sendingBy = null) => await SendAsyncMailMessageInvoke(parameters, sendingBy);

        public async Task SendAsyncSecureEMail(BaseEmailParameters parameters, string sendingBy = null) => await SendAsyncMailMessageInvoke(parameters, sendingBy);


        private MailMessage BuildMessage(BaseEmailParameters parameters) => parameters is StaticTextEmailParameters
                ? new StaticTextMailMessageBuilder().BuildMessage(parameters)
                : new TemplateMailMessageBuilder(_templatePath).BuildMessage(parameters);

        private async Task SendAsyncMailMessageInvoke(BaseEmailParameters parameters, string sendingBy)
        {
            var recipients = parameters.GetToRecipients();

            if (parameters.IsSeparateEmailForEveryRecipient && recipients.Count > 1)
            {
                recipients.ForEach(async x =>
                {
                    parameters.ClearToRecipients();
                    await SendMailMessageInvoke(parameters.AddTo(x));
                });
            }
            else
            {
                await SendMailMessageInvoke(parameters);
            }
        }


        private async Task SendMailMessageInvoke(BaseEmailParameters parameters, string sendingBy = null)
        {
            var mailmessage = BuildMessage(parameters);

            await _emailProvider.SendEmailAsync(
                string.IsNullOrEmpty(sendingBy) ? mailmessage.From.Address : sendingBy,
                parameters.To,
                parameters.To,
                mailmessage.Subject,
                mailmessage.Body);
        }
    }
}
