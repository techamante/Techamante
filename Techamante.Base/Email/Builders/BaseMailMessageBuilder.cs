using System.Net.Mail;

namespace Techamante.Base.Email.Builders
{
    public abstract class BaseMailMessageBuilder {
        public abstract MailMessage BuildMessage(BaseEmailParameters parameters);
    }
}
