using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

using MQ.MClientModel;
using MQ.MClientModel.Common;

using MessageGun.DomainModel.Tools.Log;
using Microsoft.Extensions.Logging;

namespace MQ.MDomainModel
{
    public partial class MDomainModel : IDisposable
    {

        private ILogger log = LogManager.GetLogger(typeof(MClient));

        private MClient mClient;

        public delegate void MqEventDelegate(object sender, string message);
        public event MqEventDelegate MqMessageEvent;


        private TimerCallback GetMessagesFromQueueCallBack;
        private Timer timerGetMessagesFromQueue;

        public MDomainModel(MQProperties qProperties)
        {
            mClient = new MClient(qProperties);
        }

        public void SetUpQueue(string queueName, int miliceconds = 100)
        {
            //mClient.JustReadAllMessageFromQueue(queueName);

            //miliceconds = 1000 * 1000;
            //return;
            
            GetMessagesFromQueueCallBack = new TimerCallback(GetMessagesFromQueue);
            timerGetMessagesFromQueue = new Timer(GetMessagesFromQueueCallBack, queueName, 0, miliceconds);
        }

        private object synclock = new object();

        private void GetMessagesFromQueue(object obj)
        {

            Monitor.Enter(synclock);

            string queueName = obj.ToString();
            try
            {
                string message = mClient.GetMessageFromQueue(queueName);

                if (message != null)
                {
                    MessageOnHandler(message);
                }
            }
            catch (Exception ex)
            {
                //log.LogError(ex.Message);
            }

            Monitor.Exit(synclock);
        }

        private void MessageOnHandler(string message)
        {
            if (MqMessageEvent != null)
            {
                MqMessageEvent(this, message);
            }

        }

        public void Dispose()
        {
            if (timerGetMessagesFromQueue != null)
            {
                timerGetMessagesFromQueue.Dispose();
            }


            mClient.Dispose();
        }
    }
}
