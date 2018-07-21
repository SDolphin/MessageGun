using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.TeleGun.Common
{
    public class PasswordEventArgs : EventArgs
    {
        private string _password;

        public string Password
        {
            get => _password;
            set => _password = value;
        }
    }
}
