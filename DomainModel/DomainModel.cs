using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MQ.MDomainModel;

using MQ.MClientModel.Common;

using Telegram.TeleDomainModel;
using Telegram.TeleDomainModel.Common;

//using TeleGun.Command;

using MessageGun.DomainModel.Tools;
//using MessageGun.DomainModel.Tools;
using MessageGun.DomainModel.Tools.Converters;
using MessageGun.DomainModel.Tools.Log;
using Microsoft.Extensions.Logging;

using MessageGun.DomainModel.Common;

using MessageGun.DomainModel.DB;
using MessageGun.DomainModel.DB.Common;
using WhatsApp.WabDomainModel;
using MessageGun.DomainModel.Tools.Messaging;

namespace MessageGun.DomainModel
{
    public class DomainModel
    {

        private ILogger log = LogManager.GetLogger(typeof(DomainModel));

        QueueWithtEvents<string> MqQueue;

        MDomainModel mDomain;
        TeleDomain teleDomain;
        RegularParser regular;
        PhoneChecker phoneChecker;


        IMessagingDomain messagingDomain;

        EntityDataBaseModel easyDataBaseModel;
        public DomainModel(DBProperties dBProperties)
        {


            try
            {
                easyDataBaseModel = new EntityDataBaseModel(dBProperties);
                log.LogInformation("Connected to DataBase");
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
            }
                
            IDictionary<int,string> dictOfAvailiablePhones = easyDataBaseModel.GetListOfAvaliablePhones();

            regular = new RegularParser();
            phoneChecker = new PhoneChecker(dictOfAvailiablePhones);

        }

        public void StartMqDomain(MQProperties mQProperties , string Queue,int miliseconds = 100)
        {
            log.LogInformation("Starting Domian model for MQ");
            try
            {
                mDomain = new MDomainModel(mQProperties);
            }
            catch (Exception ex)
            {
                log.LogCritical("Domain model critical in MQ constuctor",ex);
            }

            log.LogInformation("Set up queue");

            mDomain.MqMessageEvent += MDomain_MqMessageEvent;
            mDomain.SetUpQueue(Queue, miliseconds);
            

        }


        private WabDomainModel wabDomainModel;

        public void StartWhatsAppDomain(string token, string domainPhone, int miliseconds = 3000)
        {
            log.LogInformation("Starting Domian model for WhatsApp");

            messagingDomain = new WabDomainModel(token, domainPhone);
         
            log.LogInformation("Start dequeuing");
            wabDomainModel.StartDequeue(miliseconds);
        }


        public void StartTeleDomain(int apiId, string apiHash, string domainPhone, Func<string> func, bool newSession = false, int miliseconds = 3000)
        {
            log.LogInformation("Starting Domian model for TeleGram");

            messagingDomain = new TeleDomain(apiId, apiHash, newSession);
            messagingDomain.DomainMessageHanler += MessagingDomain_DomainMessageHanler;

            var ConnectionModule = (TeleDomain)messagingDomain;
           

            log.LogInformation("Connection start");
            bool connect;
            connect = ConnectionModule.Connect(domainPhone, func);

            if (connect)
            {
                log.LogInformation("Connected");
            }
            else
            {
                log.LogError("Telegram not connected");
            }


            log.LogInformation("Start dequeuing");
            teleDomain.StartDequeue(miliseconds);
        }

       
        private void MDomain_MqMessageEvent(object sender, string message)
        {


           // log.LogInformation("Enqued Message from ");

            string pavel = "79772696869";
            string ageev = "79162403666";
            string my = "79169557171";

            //string message = MqQueue.Dequeue();

            IDictionary<string,string> dictionary = regular.Parse(message);

            TmessageD tmessage = new TmessageD(dictionary,false);

            int id__ = easyDataBaseModel.InsertIntoMqMessages(tmessage);
            tmessage.Id = id__;

            if (phoneChecker.IsAvaliable(tmessage.PhoneNumber) >= 0)
            {
                if (tmessage.Time.Date == DateTime.Today)
                {

                    if (tmessage.PhoneNumber == ageev)
                    {
                        if (tmessage.Body.Length != 0)
                        {
                            log.LogInformation("Sended to " + tmessage.PhoneNumber);
                            //teleDomain.AddMessageToQueue(tmessage);
                            messagingDomain.AddMessageToQueue(tmessage);
                        }
                        else
                        {
                            log.LogError($"Message Body is empty", message);
                        }
                    }
                    
                    /// Костыль для проверки сообщений
                    // if (tmessage.PhoneNumber == ageev)              
                    //{

                    //    TmessageD tmessage1 = new TmessageD(dictionary, true);

                    //    tmessage1.Id = id__;

                    //    tmessage1.PhoneNumber = my;
                    //    if (tmessage1.Body.Length != 0)
                    //    {
                    //        log.LogInformation("Sended to " + tmessage1.PhoneNumber);
                    //        //teleDomain.AddMessageToQueue(tmessage1);
                            
                    //    }
                    //    else
                    //    {
                    //        log.LogError($"Message Body is empty", message);
                    //    }

                    //}
                    

                }
                else
                {
                    log.LogWarning("Message out of date");
                    easyDataBaseModel.InsertIntoTeleMessages(tmessage.Id, false, "Message out of date");
                }
            }
            //else
            {
                //TODO записываем в базу
                //log.LogInformation("Message out of Avaliable");                
            }
        }


        private void MessagingDomain_DomainMessageHanler(object obj, MessageDomainEventArgs domainEventArgs)
        {
            

            if (domainEventArgs.status == Status.Sended)
            {
                log.LogInformation($"Message sended with status {domainEventArgs.status}");
                easyDataBaseModel.InsertIntoTeleMessages(domainEventArgs.phoneInfo.Id, true);
            }
            else
            {
                log.LogError($"Message sended with status {domainEventArgs.status}");
                easyDataBaseModel.InsertIntoTeleMessages(domainEventArgs.phoneInfo.Id, false, domainEventArgs.Description);
            }
        }


        public void Dispose()
        {
            mDomain.MqMessageEvent -= MDomain_MqMessageEvent;
            mDomain.Dispose();
            log.LogInformation($"Disposed MQ");
            //teleDomain.Dispose();
            //wabDomainModel.Dispose();

            messagingDomain.Dispose();

            log.LogInformation($"Disposed Telegram");
        }

    }

    
    

}
