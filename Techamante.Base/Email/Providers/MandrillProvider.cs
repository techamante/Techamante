using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandrill;
using Mandrill.Models;
using Mandrill.Requests.Messages;

namespace Techamante.Email.Providers
{
    public class MandrillProvider:IEmailProvider
    {

        private bool _isInitialized;
        private string _apiKey = "";
        private bool _isEmailEnabled;  //default to false

        /// <summary>
        /// Sends an email using Mandrill.com
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <param name="toEmail"></param>
        /// <param name="toName"></param>
        /// <param name="subject"></param>
        /// <param name="bodyHtml"></param>
        public async Task SendEmailAsync(string fromEmail, string toEmail, string toName, string subject, string bodyHtml)
        {
            var mandrillApi = new MandrillApi(ApiKey);
            var result = await mandrillApi.SendMessage(new SendMessageRequest(new EmailMessage
            {
                To = new List<EmailAddress> { new EmailAddress { Email = toEmail, Name = toName } },
                FromEmail = fromEmail,
                Subject = subject,
                Html = bodyHtml
            }));



            if (result != null)
            {
                switch (result[0].Status)
                {
                    case EmailResultStatus.Sent:
                        break;

                    default:
                        throw new Exception($"Email not sent:  {result[0].Status}");
                }
            }
        }

        private string ApiKey => string.IsNullOrEmpty(_apiKey)
            ? Microsoft.Azure.CloudConfigurationManager.GetSetting(Constants.APIKEY_SETTINGKEY_PREFIX + Constants.EMAIL_APIKEY_SETTINGKEY)
            : _apiKey;

        private bool IsEmailEnabled
        {
            get
            {
                if (!_isInitialized)
                {
                    _isEmailEnabled = Microsoft.Azure.CloudConfigurationManager.GetSetting(Constants.APIKEY_SETTINGKEY_PREFIX + Constants.IS_EMAIL_ENABLED_SETTINGKEY) == "true";
                    _isInitialized = true;
                }
                return _isEmailEnabled;
            }
        }
    }
}
