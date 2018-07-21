using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageGun.DomainModel.Tools.Messaging
{
    public interface IPhoneInfo
    {
        int Id { get; }
        string PhoneNumber { get; }
        string Message { get; }
    }
}
