using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.TeleGun.Common
{
    public interface ITMessage
    {
        string PhoneNumber { get;  }
        string Message { get;  }
    }


}
