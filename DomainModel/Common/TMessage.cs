using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//using Telegram.TeleGun.Common;
using Telegram.TeleDomainModel.Common;
using MessageGun.DomainModel.DB.Common.Interfaces;
using MessageGun.DomainModel.Tools.Converters;

using MessageGun.DomainModel.Tools.Messaging;

namespace MessageGun.DomainModel.Common
{
    public class Tmessage : IPhoneInfo , IMQuery
    {
        protected string _phoneNumber;
        protected string _message;
        private int _id;


        protected DateTime _time;
        protected string _header;
        protected string _body;
       


        public Tmessage(MqMessageXmlClass mqMessageXml)
        {
            _phoneNumber = mqMessageXml.Recipient.Address;

            _time = DateTime.Parse(mqMessageXml.RqTm);

            _header = mqMessageXml.Message.Header;
            _body = mqMessageXml.Message.Body;

            _message = string.Concat(_header, _body);

        }

        public  Tmessage(IDictionary<string, string> dictionary)
        {
            _phoneNumber = dictionary["Phone"];

            _time = DateTime.Parse(dictionary["Time"]);

            _header = dictionary["Header"];
            _body = dictionary["Body"];

            _message = string.Concat(_header, _body);

        }

        public int Id
        {
            get => _id;
            set => _id = value;
        }


        public  DateTime Time
        {
            get => _time;
        }

        public string Header
        {
            get => _header;
        }

        public string Body
        {
            get => _body;

        }

        public  string PhoneNumber
        {
            get => _phoneNumber;
            set => _phoneNumber = value;
        }
        public string Message
        {
            get => _message;
            //set => _message = value;
        }
    }


    

}
