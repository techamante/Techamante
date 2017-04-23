using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Techamante.Email {
	public abstract class BaseEmailParameters {
		protected BaseEmailParameters(string emailTypeName) {
			EmailTypeName = emailTypeName;
			From = DefaultFrom;
		}

		protected string DefaultFrom { get; private set; }

		public string EmailTypeName { get; private set; }

		public string From { get; set; }

		public string To { get; set; }

		public string CC { get; set; }

		public string BCC { get; set; }

		public string Subject { get; set; }

		public string Body { get; set; }

		public bool IsHtml { get; set; }

		public bool IsSeparateEmailForEveryRecipient { get; set; }

		public virtual IList<Attachment> Attachments { get; } = new List<Attachment>();

		public BaseEmailParameters AddTo(string mailAddress) {
			if (String.IsNullOrEmpty(mailAddress))
				return this;

			To = !String.IsNullOrEmpty(To) ? String.Join(",", To, mailAddress) : mailAddress;

			return this;
		}

		public BaseEmailParameters AddTo(IEnumerable<string> mailAddresses) {
			if (mailAddresses == null)
				return this;

			foreach (var mailAddress in mailAddresses)
				To = !String.IsNullOrEmpty(To) ? String.Join(",", To, mailAddress) : mailAddress;

			return this;
		}

		public BaseEmailParameters AddCC(string mailAddress) {
			if (String.IsNullOrEmpty(mailAddress))
				return this;

			CC = !String.IsNullOrEmpty(CC) ? String.Join(",", CC, mailAddress) : mailAddress;

			return this;
		}

		public BaseEmailParameters AddBCC(string mailAddress) {
			if (String.IsNullOrEmpty(mailAddress))
				return this;

			BCC = !String.IsNullOrEmpty(BCC) ? String.Join(",", BCC, mailAddress) : mailAddress;

			return this;
		}


		public BaseEmailParameters WithSeparateEmailForEveryRecipient() {
			IsSeparateEmailForEveryRecipient = true;
			return this;
		}
        
		public List<string> GetToRecipients() {
			var recipients = new List<string>();

			if (String.IsNullOrEmpty(To))
				return recipients;

			var toEmailsList = To.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var emailAddress in toEmailsList) {
				if (!String.IsNullOrEmpty(emailAddress) && !recipients.Contains(emailAddress))
					recipients.Add(emailAddress);
			}

			return recipients;
		}

		public void ClearToRecipients() => To = null;
	}

	public class EmailParameterAttribute : Attribute {
		public EmailParameterAttribute(string parameterName) {
			ParameterName = parameterName;
		}

		public string ParameterName { get; set; }
	}

	public class EmailTemplateAttribute : Attribute {
		public EmailTemplateAttribute(string templateName) {
			TemplateName = templateName;
		}

		public string TemplateName { get; set; }
	}
}
