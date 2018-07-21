using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBM.WMQ;
using IBM.XMS.Client.WMQ.Messages;

using Microsoft.Extensions.Logging;
using MQ.MClientModel.Common;

using MessageGun.DomainModel.Tools.Log;

using System.Collections;
using System.Threading;

namespace MQ.MClientModel
{

    /// <summary>
    /// Easy library for working with MQ queue, just connect and get 
    /// </summary>
    public class MClient : IDisposable
    {

        private MQQueue queue;
        private MQMessage queueMessage;
        private MQQueueManager queueManager;

        private ILogger log = LogManager.GetLogger(typeof(MClient));

        //public delegate void MqEvenenDelegate(object sender,string message);
        //public event MqEvenenDelegate MqMessageEvent;

        //private IQProperties qProperties;
        //private TimerCallback GetMessagesFromQueueCallBack;
       // private Timer timerGetMessagesFromQueue;


        public MClient(MQProperties qProperties)
        {
            
            try
            {
                Hashtable queueProp = qProperties.GetQueueConnectionProperties();
                queueManager = new MQQueueManager(qProperties.QueueManagerName, queueProp);

                //GetMessagesFromQueueCallBack = new TimerCallback(GetMessagesFromQueue);
            }
            catch (MQException mqexp)
            {
                log.LogError(mqexp.Message);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
            }

        }


        public string queueName;




        //public void SetUpMessageQueueEvents(string queueName, int miliseconds = 100)
        //{

        //    timerGetMessagesFromQueue = new Timer(GetMessagesFromQueueCallBack, queueName, 0, miliseconds);

        //}


        /// <summary>
        /// Getting element from queue if it is not empty
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns>null if queue empty </returns>
        public string GetMessageFromQueue(string queueName)
        {
            
            try
            {
                queue = queueManager.AccessQueue(queueName, MQC.MQOO_INPUT_AS_Q_DEF + MQC.MQOO_FAIL_IF_QUIESCING);
                queueMessage = new MQMessage();
                queueMessage.Format = MQC.MQFMT_STRING;

                queue.Get(queueMessage);
                string message = queueMessage.ReadString(queueMessage.MessageLength);

                queue.Close();
                return message;
                
            }
            catch (MQException mqexp)
            {
                return null;
                log.LogInformation(string.Concat("MQQueue::Get ended with ", mqexp.Message));
            }
          
        }



        public List<string> GetAllMessagesFromQueue(string queueName)
        {
            List<string> listOfMessages = new List<string>();
            queue = queueManager.AccessQueue(queueName, MQC.MQOO_INPUT_AS_Q_DEF + MQC.MQOO_FAIL_IF_QUIESCING);

            while (true)
            {
                try
                {

                    queueMessage = new MQMessage();
                    queueMessage.Format = MQC.MQFMT_STRING;

                    queue.Get(queueMessage);
                    string message = queueMessage.ReadString(queueMessage.MessageLength);

                    queue.Close();

                    listOfMessages.Add(message);

                }
                catch (MQException mqexp)
                {
                    break;
                    log.LogInformation(string.Concat("MQQueue::Get ended with ", mqexp.Message));
                }

            }

            return listOfMessages;
        }



        public string JustReadLastMessageFromQueue(string queueName)
        {

            try
            {
                queue = queueManager.AccessQueue(queueName, MQC.MQOO_BROWSE + MQC.MQOO_FAIL_IF_QUIESCING);
                queueMessage = new MQMessage();
                queueMessage.Format = MQC.MQFMT_STRING;

                MQGetMessageOptions mQGetMessageOptions = new MQGetMessageOptions();
                mQGetMessageOptions.Options = MQC.MQGMO_BROWSE_FIRST;

                queue.Get(queueMessage, mQGetMessageOptions);
                string message = queueMessage.ReadString(queueMessage.MessageLength);

                queue.Close();
                return message;

            }
            catch (MQException mqexp)
            {
                return null;
                log.LogInformation(string.Concat("MQQueue::Get ended with ", mqexp.Message));
            }

        }


        public List<string> JustReadAllMessageFromQueue(string queueName)
        {

            List<string> listOfMessages = new List<string>();

            queue = queueManager.AccessQueue(queueName, MQC.MQOO_BROWSE + MQC.MQOO_FAIL_IF_QUIESCING);
           

            while (true)
            {

                try
                {

                    queueMessage = new MQMessage();
                    queueMessage.Format = MQC.MQFMT_STRING;

                    MQGetMessageOptions mQGetMessageOptions = new MQGetMessageOptions();
                    //mQGetMessageOptions.Options = MQC.MQGMO_BROWSE_FIRST;

                    mQGetMessageOptions.Options = MQC.MQGMO_BROWSE_NEXT + MQC.MQGMO_NO_WAIT + MQC.MQGMO_FAIL_IF_QUIESCING;



                    queue.Get(queueMessage, mQGetMessageOptions);

                    //mQGetMessageOptions = new MQGetMessageOptions();
                    //mQGetMessageOptions.Options = MQC.MQGMO_BROWSE_NEXT;

                    listOfMessages.Add(queueMessage.ReadString(queueMessage.MessageLength));

                }
                catch (MQException mqexp)
                {
                    break;
                }
            }

            queue.Close();

            return listOfMessages;

        }



        public IEnumerable JustReadYieldAllMessageFromQueue(string queueName)
        {

            queue = queueManager.AccessQueue(queueName, MQC.MQOO_BROWSE + MQC.MQOO_FAIL_IF_QUIESCING);

            while (true)
            {
                try
                {
                    queueMessage = new MQMessage();
                    queueMessage.Format = MQC.MQFMT_STRING;

                    MQGetMessageOptions mQGetMessageOptions = new MQGetMessageOptions();
                    //mQGetMessageOptions.Options = MQC.MQGMO_BROWSE_FIRST;

                    mQGetMessageOptions.Options = MQC.MQGMO_BROWSE_NEXT + MQC.MQGMO_NO_WAIT + MQC.MQGMO_FAIL_IF_QUIESCING;

                    queue.Get(queueMessage, mQGetMessageOptions);

                }
                catch (MQException mqexp)
                {
                    queue.Close();
                    yield break;
                }

                yield return queueMessage.ReadString(queueMessage.MessageLength);

            }
        }

        public IEnumerable GetYieldAllMessagesFromQueue(string queueName)
        {
            
            queue = queueManager.AccessQueue(queueName, MQC.MQOO_INPUT_AS_Q_DEF + MQC.MQOO_FAIL_IF_QUIESCING);
            string message = string.Empty;
            while (true)
            {
                try
                {

                    queueMessage = new MQMessage();
                    queueMessage.Format = MQC.MQFMT_STRING;

                    queue.Get(queueMessage);
                    message = queueMessage.ReadString(queueMessage.MessageLength);

                }
                catch (MQException mqexp)
                {
                    queue.Close();
                    yield break;
                    log.LogInformation(string.Concat("MQQueue::Get ended with ", mqexp.Message));
                }
                yield return message;
            }

            
        }




        public string WriteMsg(string QueueName , string strInputMsg)
        {
            string strReturn = "";
            try
            {
                queue = queueManager.AccessQueue(QueueName,
                MQC.MQOO_OUTPUT + MQC.MQOO_FAIL_IF_QUIESCING);
                string message = strInputMsg;
                queueMessage = new MQMessage();
                queueMessage.WriteString(message);
                queueMessage.Format = MQC.MQFMT_STRING;
                MQPutMessageOptions queuePutMessageOptions = new MQPutMessageOptions();
                queue.Put(queueMessage, queuePutMessageOptions);
                strReturn = "Message sent to the queue successfully";
            }
            catch (MQException MQexp)
            {
                strReturn = "Exception: " + MQexp.Message;
            }
            catch (Exception exp)
            {
                strReturn = "Exception: " + exp.Message;
            }
            return strReturn;
        }


        public void Dispose()
        {
            //timerGetMessagesFromQueue.Dispose();
            queueManager.Disconnect();
            queueManager.Close();

            queue.Close();
        }




    }
}
