using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageGun.DomainModel.DB.Common.Interfaces
{
    public interface IMQuery
    {
        string PhoneNumber { get; }
        DateTime Time { get; }
        string Header { get;}
        string Body { get; }
    }

    public interface ITQuery : IMQuery
    {
        bool IsSended { get; }
        string Desription { get; }
    }
}
