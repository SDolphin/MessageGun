using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.MClientModel.Common
{
    public class PropertiesException : Exception
    {
       public PropertiesException()
    : base() { }

        public PropertiesException(string message)
            : base(message) { }

        public PropertiesException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public PropertiesException(string message, Exception innerException)
            : base(message, innerException) { }

        public PropertiesException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

    }
}
