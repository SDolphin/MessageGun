using MessageGun.DomainModel.Tools.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace MessageGun.DomainModel.Common
{
    public class TmessageD : Tmessage
    {

        private bool _isDebug;
        

        public TmessageD(IDictionary<string, string> dictionary, bool isDebag = false) : base(dictionary)
        {
            _isDebug = isDebag;

            _phoneNumber = dictionary["Phone"];
            _time = DateTime.Parse(dictionary["Time"]);
            _header = dictionary["Header"];
            _body = PreapreSexyBody(dictionary["Body"]);

            _message = PreapreSexyMessage(_header, _body, isDebag);

        }

        public TmessageD(MqMessageXmlClass mqMessageXml, bool isDebag = false) : base(mqMessageXml)
        {
            _isDebug = isDebag;

            _phoneNumber = mqMessageXml.Recipient.Address;
            _time = DateTime.Parse(mqMessageXml.RqTm);
            _header = mqMessageXml.Message.Header;
            _body = mqMessageXml.Message.Body;

            _message = PreapreSexyMessage(_header, _body, isDebag);

        }


        private string PreapreSexyMessage(string header, string body, bool isDebag)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (header.Length != 0)
            {
                stringBuilder.AppendLine(header);
                stringBuilder.AppendLine();
            }
            
            stringBuilder.AppendLine(body);

            if (isDebag)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Debug info :");
                stringBuilder.AppendLine(_time.ToString());
                stringBuilder.AppendLine(DateTime.Now.ToString());
            }
            return stringBuilder.ToString();
        }

        private string PreapreSexyBody(string s)
        {
            return s.Replace("&quot;", "\"");
        }
    }
}
