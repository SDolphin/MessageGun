using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.MClientModel.Common
{
    public interface IQProperties
    {
        string QueueManagerName { get; set; }
        string Host { get; set; }
        int Port { get; set; }
        string Chanel { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        //string QueueName { get; set; }

        //Hashtable GetQueueConnectionProperties();
    }
}
