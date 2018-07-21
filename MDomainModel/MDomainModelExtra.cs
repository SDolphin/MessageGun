using MQ.MClientModel;
using MQ.MClientModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.MDomainModel
{
    public partial class MDomainModel 
    {

        public string ReadLastMessage(string queueName)
        {
            return mClient.JustReadLastMessageFromQueue(queueName);
        }

        public List<string> ReadAllMessages(string queueName)
        {
            return mClient.JustReadAllMessageFromQueue(queueName);
        }

        public int Count(string queueName)
        {
            return mClient.JustReadAllMessageFromQueue(queueName).Count;
        }

        public string GetLastMessage(string queueName)
        {
            return mClient.GetMessageFromQueue(queueName);
        }

        public List<string> GetAllMessage(string queueName)
        {
            return mClient.GetAllMessagesFromQueue(queueName);
        }

        public IEnumerable<string> ReadYieldMessages(string queueName)
        {
            //return mClient.GetAllMessagesFromQueue(queueName);

            foreach (string s in mClient.JustReadYieldAllMessageFromQueue(queueName))
            {
                 yield return s;
            }

        }


        public IEnumerable<string> GetYieldMessages(string queueName)
        {
            //return mClient.GetAllMessagesFromQueue(queueName);

            foreach (string s in mClient.GetYieldAllMessagesFromQueue(queueName))
            {
                yield return s;
            }

        }



    }
}
