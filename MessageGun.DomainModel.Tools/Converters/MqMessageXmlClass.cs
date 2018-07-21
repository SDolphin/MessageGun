using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MessageGun.DomainModel.Tools.Converters
{
    [Serializable()]
    [XmlRoot("NotificationRq", Namespace = "http://croc.ru/cppk/esb/messages")]
    public class MqMessageXmlClass
    {
        [XmlElement(ElementName = "RqUID")]
        public string RqUID { get; set; }

        [XmlElement(ElementName = "RqTm")]
        public string RqTm { get; set; }

        [XmlElement(ElementName = "SourceSystem")]
        public string SourceSystem { get; set; }

        [XmlElement(ElementName = "Recipient")]
        public RecipientClass Recipient { get; set; }

        [XmlElement(ElementName = "Message")]
        public MessageClass Message { get; set; }

        [XmlElement(ElementName = "Sync")]
        public bool Sync { get; set; }
    }

    public  class RecipientClass
    {
        [XmlElement(ElementName = "RecipientType")]
        public string RecipientType { get; set; }

        [XmlElement(ElementName = "Address")]
        public string Address { get; set; }
    }

    public class MessageClass
    {
        [XmlElement(ElementName = "Header")]
        public string Header { get; set; }

        [XmlElement(ElementName = "Body")]
        public string Body { get; set; }
    }
}
