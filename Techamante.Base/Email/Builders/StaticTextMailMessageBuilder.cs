using System.Net.Mail;

namespace Techamante.Email.Builders
{
    public class StaticTextMailMessageBuilder : BaseMailMessageBuilder {
        public override MailMessage BuildMessage(BaseEmailParameters parameters) {
            var mail = new MailMessage { From = new MailAddress(parameters.From) };
            mail.To.Add(new MailAddress(parameters.To));
            mail.Bcc.Add(new MailAddress(parameters.BCC));
            mail.CC.Add(new MailAddress(parameters.CC));
            mail.Subject = parameters.Subject;
            mail.Body = parameters.Body;
            mail.IsBodyHtml = parameters.IsHtml;

            if (parameters.Attachments != null && parameters.Attachments.Count > 0) {
                foreach (var attachment in parameters.Attachments)
                    mail.Attachments.Add(attachment);
            }

            return mail;
        }
    }
}
