using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessageGun.DomainModel.Tools;

namespace WhatsApp.Waboxapp.Common
{
    public class WabCon
    {
        private string _token;
        private string _phoneFrom;

        public string Token { get => _token; }
        public string PhoneFrom { get => _phoneFrom; }

        public WabCon(string token, string phone)
        {
            _token = token;
            _phoneFrom = TextHelper.PrepareAndCheckPhone(phone);
        }


    }
}
