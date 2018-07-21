using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

using Telegram.TeleGun.Common;
using Telegram.TeleGun;

using Telegram.TeleDomainModel.Common;

using MessageGun.DomainModel.Tools.Messaging;

using MessageGun.DomainModel.Tools.Log;
using Microsoft.Extensions.Logging;

namespace Telegram.TeleDomainModel
{

    public class TeleDomain : MessagingDomain ,  IMessagingDomain
    {

        private ILogger log = LogManager.GetLogger(typeof(TeleDomain));

        private TLGun tLGun;
        private Func<string> actionPassword;

        public TeleDomain(int apiId, string apiHash, bool newSession = false)
        {
            tLGun = new TLGun(apiId, apiHash, newSession);
            tLGun.PassReturnEvent += TLGun_PassReturnEvent;
        }

        /// <summary>
        /// Easy connect
        /// </summary>
        /// <param name="domainPhone"></param>
        /// <param name="actionPassword"> func wich get password </param>
        /// <returns></returns>
        public bool Connect(string domainPhone, Func<string> actionPassword)
        {
            this.actionPassword = actionPassword;

            try
            {

                if (tLGun.Connect(domainPhone))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return false;
            }
        }


        public override MessageDomainEventArgs Send(IPhoneInfo phoneInfo)
        {
            MessageDomainEventArgs messageDomainEventArgs = new MessageDomainEventArgs();

            try
            {
                TryToSendWithReconnect(phoneInfo);
                messageDomainEventArgs.status = Status.Sended;
            }
            catch (Exception ex)
            {
                messageDomainEventArgs.status = Status.NotSened;
            }

            return messageDomainEventArgs;
        }

        private void TryToSendWithReconnect(IPhoneInfo phoneInfo)
        {
            try
            {
                var rezz = tLGun.SendMessageTo(phoneInfo);
            }
            catch (Exception ex)
            {
                log.LogWarning("Message did not send, reconnection start");

                //timerGetMessagesFromQueue.Change(Timeout.Infinite, Timeout.Infinite);
                tLGun.Reconnect();
                //timerGetMessagesFromQueue.Change(0, _miliseconds);

                log.LogInformation("Reconnection Complite");
                var rezz = tLGun.SendMessageTo(phoneInfo);
            }

        }


        /// <summary>
        /// Invoke our Sended Func wich getting password
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="eventArgs"></param>
        private void TLGun_PassReturnEvent(object obj, PasswordEventArgs eventArgs)
        {
            eventArgs.Password = actionPassword.Invoke();
        }

        public override void Dispose()
        {
            tLGun.Dispose();
            tLGun.PassReturnEvent -= TLGun_PassReturnEvent;
            timerGetMessagesFromQueue.Dispose();
        }

    }


    /// <summary>
    /// Library for concurrent adding messages to send with time interval.
    /// We don't want to be blocked)
    /// </summary>
    /*public class TeleDomain : IDisposable
    {
        private ILogger log = LogManager.GetLogger(typeof(TeleDomain));


        public delegate void TeleDomainEventArgsHandler(object obj, TeleDomainEventArgs domainEventArgs);
        public event TeleDomainEventArgsHandler TeleDomainMessageHanler;
        
        public ConcurrentQueue<IPhoneInfo> concurrentSendingQueue = new ConcurrentQueue<IPhoneInfo>();

        private TLGun tLGun;

        private TimerCallback GetMessagesFromQueueCallBack;
        private Timer timerGetMessagesFromQueue;
        //private ManualResetEvent resetEvent;


        private Func<string> actionPassword;

        private int _miliseconds;

        public TeleDomain(int apiId, string apiHash,  bool newSession = false)
        {
            tLGun = new TLGun(apiId, apiHash, newSession);
            tLGun.PassReturnEvent += TLGun_PassReturnEvent;
            //resetEvent = new ManualResetEvent(false);
        }


        protected void OnMessageEvent(TeleDomainEventArgs domainEventArgs)
        {
            if (TeleDomainMessageHanler != null)
            {
                TeleDomainMessageHanler(this, domainEventArgs);
            }
        }

        /// <summary>
        /// Easy connect
        /// </summary>
        /// <param name="domainPhone"></param>
        /// <param name="actionPassword"> func wich get password </param>
        /// <returns></returns>
        public bool Connect(string domainPhone, Func<string> actionPassword)
        {
            this.actionPassword = actionPassword;

            try
            {

                if (tLGun.Connect(domainPhone))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return false;
            }
        }


        public void AddMessageToQueue(IPhoneInfo phoneInfo)
        {
            concurrentSendingQueue.Enqueue(phoneInfo);
        }


        /// <summary>
        /// Go
        /// </summary>
        /// <param name="miliseconds"> Speed with wich we wll send messages</param>
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
                //lock (synclock)
                //{
                    TryToSend(phoneInfo);
                //}
            }
            else
            {
               // log.LogWarning("Dequeiuing returned false");
            }
        }

        private object synclock = new object();

        /// <summary>
        /// Here we send Message and prepare eventarg informatoin about status of sened message
        /// </summary>
        /// <param name="phoneInfo"></param>
        private void TryToSend(IPhoneInfo phoneInfo)
        {
            Monitor.Enter(synclock);
 
            TeleDomainEventArgs domainEventArgs = new TeleDomainEventArgs();
            domainEventArgs.phoneInfo = phoneInfo;
            try
            {

                //timerGetMessagesFromQueue.Change(Timeout.Infinite, Timeout.Infinite);
                //lock (synclock)
                {
                    TryToSendWithReconnect(phoneInfo);
                }
                //timerGetMessagesFromQueue.Change(0, _miliseconds);
                //resetEvent.Set();

                //var rezz = tLGun.SendMessageTo(phoneInfo);
                domainEventArgs.status = Status.Sended;
            }
            catch (Exception ex)
            {
                log.LogError("Message not sended :", ex);
                domainEventArgs.status = Status.NotSened;
                domainEventArgs.Description = ex.Message;
            }
            Monitor.Exit(synclock);
            OnMessageEvent(domainEventArgs);
        }

        private void TryToSendWithReconnect(IPhoneInfo phoneInfo)
        { 
            try
            {
                var rezz = tLGun.SendMessageTo(phoneInfo);
            }
            catch (Exception ex)
            {
                log.LogWarning("Message did not send, reconnection start");

                //timerGetMessagesFromQueue.Change(Timeout.Infinite, Timeout.Infinite);
                tLGun.Reconnect();
                //timerGetMessagesFromQueue.Change(0, _miliseconds);

                log.LogInformation("Reconnection Complite");
                var rezz = tLGun.SendMessageTo(phoneInfo);
            }
            
        }

        /// <summary>
        /// Invoke our Sended Func wich getting password
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="eventArgs"></param>
        private void TLGun_PassReturnEvent(object obj, PasswordEventArgs eventArgs)
        {
            eventArgs.Password = actionPassword.Invoke();
        }

        public void Dispose()
        {
            tLGun.Dispose();
            tLGun.PassReturnEvent -= TLGun_PassReturnEvent;
            timerGetMessagesFromQueue.Dispose();
        }
    }*/
}
