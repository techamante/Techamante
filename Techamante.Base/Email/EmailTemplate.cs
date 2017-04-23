using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Techamante.Email
{
    [XmlRoot("EmailNotificationTemplate")]
    public class EmailNotificationTemplate {
        [XmlElement("From")]
        public string From { get; set; }
        [XmlElement("To")]
        public string To { get; set; }
        [XmlElement("CC")]
        public string CC { get; set; }
        [XmlElement("BCC")]
        public string BCC { get; set; }
        [XmlElement("Subject")]
        public string Subject { get; set; }
        [XmlElement("IsHtml")]
        public bool? IsHtml { get; set; }

        private string _body;
        [XmlElement("Body")]
        public XmlCDataSection BodyCDATA {
            get {
                return new XmlDocument().CreateCDataSection(_body);
            }
            set {
                _body = value.Value;
            }
        }
        [XmlIgnore]
        public string Body {
            get {
                return _body;
            }
            set {
                _body = value;
            }
        }

        private static XmlSerializer _serializer;
        [Obsolete]
        public static EmailNotificationTemplate Load(string filename) {
            if (_serializer == null)
                _serializer = new XmlSerializer(typeof(EmailNotificationTemplate));

            using (var stream = new StreamReader(filename)) {
                return _serializer.Deserialize(stream) as EmailNotificationTemplate;
            }
        }
    }
}
