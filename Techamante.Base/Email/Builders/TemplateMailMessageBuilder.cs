using System;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text.RegularExpressions;
using Techamante.Base.Caching;


namespace Techamante.Base.Email.Builders
{
    public class TemplateMailMessageBuilder : BaseMailMessageBuilder
    {
        private static readonly Regex MailPartPattern = new Regex("\\%\\%([A-Za-z0-9_.]+)\\%\\%");

        private string _templatePath;
        public TemplateMailMessageBuilder(string templatePath)
        {
            _templatePath = templatePath;

        }

        public override MailMessage BuildMessage(BaseEmailParameters parameters)
        {
            var template = GetTemplate(parameters);

            var mail = new MailMessage { From = new MailAddress(parameters.From) };

            if (!String.IsNullOrEmpty(parameters.To))
            {
                foreach (var emailAddress in parameters.To.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!String.IsNullOrEmpty(emailAddress))
                        mail.To.Add(new MailAddress(emailAddress));
                }
            }
            if (!String.IsNullOrEmpty(parameters.BCC))
            {
                foreach (var emailAddress in parameters.BCC.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!String.IsNullOrEmpty(emailAddress))
                        mail.Bcc.Add(new MailAddress(emailAddress));
                }
            }
            if (!String.IsNullOrEmpty(parameters.CC))
            {
                foreach (var emailAddress in parameters.CC.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!String.IsNullOrEmpty(emailAddress))
                        mail.CC.Add(new MailAddress(emailAddress));
                }
            }
            if (!String.IsNullOrEmpty(template.From))
            {
                mail.From = new MailAddress(template.From);
            }
            if (!String.IsNullOrEmpty(template.To))
            {
                foreach (var emailAddress in template.To.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!String.IsNullOrEmpty(emailAddress))
                        mail.To.Add(new MailAddress(emailAddress));
                }
            }
            if (!String.IsNullOrEmpty(template.BCC))
            {
                foreach (var emailAddress in template.BCC.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!String.IsNullOrEmpty(emailAddress))
                        mail.Bcc.Add(new MailAddress(emailAddress));
                }
            }
            if (!String.IsNullOrEmpty(template.CC))
            {
                foreach (var emailAddress in template.CC.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!String.IsNullOrEmpty(emailAddress))
                        mail.CC.Add(new MailAddress(emailAddress));
                }
            }

            mail.IsBodyHtml = template.IsHtml.GetValueOrDefault();
            mail.Subject = GenerateParametrizedPart(template.Subject, parameters);
            mail.Body = GenerateParametrizedPart(template.Body, parameters);

            if (parameters.Attachments != null && parameters.Attachments.Count > 0)
            {
                foreach (var attachment in parameters.Attachments)
                    mail.Attachments.Add(attachment);
            }
            return mail;
        }


        private string GenerateParametrizedPart(string mailPart, BaseEmailParameters parameters)
        {
            foreach (var match in MailPartPattern.Matches(mailPart))
            {
                mailPart = mailPart.Replace(match.ToString(), GetValueFromParameter(parameters, match.ToString().Substring(2, match.ToString().Length - 4)));
            }
            return mailPart;
        }

        private string GetValueFromParameter(BaseEmailParameters parameters, string paramName)
        {
            var property = parameters.GetType().GetProperties()
                .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(EmailParameterAttribute))
                                       && string.Compare(((prop.GetCustomAttribute(typeof(EmailParameterAttribute))) as EmailParameterAttribute).ParameterName, paramName, StringComparison.OrdinalIgnoreCase) == 0);
            return property?.GetValue(parameters)?.ToString() ?? String.Empty;
        }

        private EmailNotificationTemplate GetTemplate(BaseEmailParameters parameters)
        {
            var templateAttr = parameters.GetType().GetCustomAttributes(typeof(EmailTemplateAttribute), true).Cast<EmailTemplateAttribute>().FirstOrDefault();

            if (templateAttr == null)
                return null;

            return ObjectFromFileCache.Instance.RetriveXmlObject<EmailNotificationTemplate>(_templatePath);
        }
    }
}
