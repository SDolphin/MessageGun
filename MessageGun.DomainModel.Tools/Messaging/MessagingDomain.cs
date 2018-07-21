using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageGun.DomainModel.Tools.Messaging
{

    public interface IMessagingDomain 
    {
        void AddMessageToQueue(IPhoneInfo phoneInfo);
        void StartDequeue(int miliseconds);
       
        event MessagingDomain.MessageDomainEventArgsHandler DomainMessageHanler;

        void Dispose();
    }

    public class MessageDomainEventArgs : EventArgs
    {
        public IPhoneInfo phoneInfo;
        public Status status;
        public string Description;
    }

    public enum Status
    {
        Sended,
        NotSened
    }


    public abstract class MessagingDomain : IDisposable , IMessagingDomain
    {

        public delegate void MessageDomainEventArgsHandler(object obj, MessageDomainEventArgs domainEventArgs);
        public event MessageDomainEventArgsHandler DomainMessageHanler;

        private TimerCallback GetMessagesFromQueueCallBack;
        public Timer timerGetMessagesFromQueue;

        public ConcurrentQueue<IPhoneInfo> concurrentSendingQueue = new ConcurrentQueue<IPhoneInfo>();

        private int _miliseconds;

        public void AddMessageToQueue(IPhoneInfo phoneInfo)
        {
            concurrentSendingQueue.Enqueue(phoneInfo);
        }


        protected void OnMessageEvent(MessageDomainEventArgs domainEventArgs)
        {
            if (DomainMessageHanler != null)
            {
                DomainMessageHanler(this, domainEventArgs);
            }
        }

        private object synclock = new object();

        public void StartDequeue(int miliseconds)
        {

            _miliseconds = miliseconds;
            GetMessagesFromQueueCallBack = new TimerCallback(Dequeuing);
            timerGetMessagesFromQueue = new Timer(GetMessagesFromQueueCallBack, null, 0, _miliseconds);

        }

        private void Dequeuing(object obj)
        {
            IPhoneInfo phoneInfo;//= new PhoneInfo();
            bool rez = concurrentSendingQueue.TryDequeue(out phoneInfo);
            if (rez == true)
            {
                TryToSend(phoneInfo);
            }
            else
            {
                // log.LogWarning("Dequeiuing returned false");
            }
        }


        private void TryToSend(IPhoneInfo phoneInfo)
        {
            Monitor.Enter(synclock);

            MessageDomainEventArgs domainEventArgs = new MessageDomainEventArgs();
            domainEventArgs.phoneInfo = phoneInfo;
            try
            {
                domainEventArgs = Send(phoneInfo);

                //domainEventArgs.status = Status.Sended;
            }
            catch (Exception ex)
            {
                //log.LogError("Message not sended :", ex);
                domainEventArgs.status = Status.NotSened;
                domainEventArgs.Description = ex.Message;
            }
            Monitor.Exit(synclock);
            OnMessageEvent(domainEventArgs);
        }

        public abstract MessageDomainEventArgs Send(IPhoneInfo phoneInfo);


        public virtual void Dispose()
        {
            timerGetMessagesFromQueue.Dispose();
        }


    }
}
