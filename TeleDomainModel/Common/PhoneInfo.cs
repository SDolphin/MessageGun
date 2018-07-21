using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.TeleGun.Common;
using Telegram.TeleDomainModel.Common;

using MessageGun.DomainModel.Tools.Messaging;

namespace Telegram.TeleDomainModel.Common
{
    public class PhoneInfo : ITMessage , IPhoneInfo
    {
        private string _phoneNumber;
        private string _message;
        private int  _id;

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => _phoneNumber = value;
        }

        public string Message
        {
            get => _message;
            set => _message = value;
        }
    }
}
