using MessageGun.DomainModel.Tools.Log;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessageGun.DomainModel.Tools.Messaging;
using System.Threading;


using Whats = WhatsApp.Waboxapp;
using WhatsApp.Waboxapp.Common;
using Telegram.TeleDomainModel.Common;

namespace WhatsApp.WabDomainModel
{
    public class WabDomainModel : MessagingDomain
    {
        private ILogger log = LogManager.GetLogger(typeof(WabDomainModel));

        private Whats.Waboxapp whatsApp;

        public WabDomainModel(string token, string fromPhone)
        {
            whatsApp = new Whats.Waboxapp(new WabCon(token, fromPhone));
        }

        public WabDomainModel(WabCon wabCon)
        {
            whatsApp = new Whats.Waboxapp(wabCon);
        }

        public override MessageDomainEventArgs Send(IPhoneInfo phoneInfo)
        {

            var status = whatsApp.SendMessage(phoneInfo.Id, phoneInfo.PhoneNumber, phoneInfo.Message);

           
            if (status == System.Net.HttpStatusCode.OK)
                return  new MessageDomainEventArgs() { status = Status.Sended };
            else
                return new MessageDomainEventArgs() { status = Status.NotSened };
        }


    }

        /*public class WabDomainModel : IDisposable
        {
            private ILogger log = LogManager.GetLogger(typeof(WabDomainModel));


            public delegate void TeleDomainEventArgsHandler(object obj, TeleDomainEventArgs domainEventArgs);
            public event TeleDomainEventArgsHandler TeleDomainMessageHanler;

            public ConcurrentQueue<IPhoneInfo> concurrentSendingQueue = new ConcurrentQueue<IPhoneInfo>();

            //private TLGun tLGun;

            private TimerCallback GetMessagesFromQueueCallBack;
            private Timer timerGetMessagesFromQueue;
            //private ManualResetEvent resetEvent;


            //private Func<string> actionPassword;

            private int _miliseconds;

            private Whats.Waboxapp whatsApp;

            public WabDomainModel(string token, string fromPhone)
            {
                whatsApp = new Whats.Waboxapp(new WabCon(token, fromPhone));
            }

            public WabDomainModel(WabCon wabCon)
            {
                whatsApp = new Whats.Waboxapp(wabCon);
            }



            protected void OnMessageEvent(TeleDomainEventArgs domainEventArgs)
            {
                if (TeleDomainMessageHanler != null)
                {
                    TeleDomainMessageHanler(this, domainEventArgs);
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

                    var status = whatsApp.SendMessage(phoneInfo.Id, phoneInfo.PhoneNumber, phoneInfo.Message);

                    if(status == System.Net.HttpStatusCode.OK)
                        domainEventArgs.status = Status.Sended;
                    else
                        domainEventArgs.status = Status.NotSened;
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





            public void Dispose()
            {
                timerGetMessagesFromQueue.Dispose();
            }
        }*/
    }
