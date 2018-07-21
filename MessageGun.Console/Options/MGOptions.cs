using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft;
using Newtonsoft.Json;

using MessageGun.Console;
using MQ.MClientModel.Common;
using MessageGun.DomainModel.DB.Common;

using System.Configuration;


namespace MessageGun.Console.Options
{
    public static class MGOptions
    {


        public static class Mq
        {
            private static string _queueName = ConfigurationManager.AppSettings["MQ ListnerQName"];
            private static int _mqInterval = Convert.ToInt32(ConfigurationManager.AppSettings["Mq Timing"]);
            private static MQProperties _mqPrpr = new MQProperties() {

                QueueManagerName = ConfigurationManager.AppSettings["MQ QueueManagerName"],
                Host = ConfigurationManager.AppSettings["MQ Host"],
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["MQ Port"]),
                Chanel = ConfigurationManager.AppSettings["MQ Chanel"],

            }; 


            public static string QueueName { get => _queueName; }
            public static MQProperties MqProp { get => _mqPrpr; }
            public static int MqInterval { get => _mqInterval; }
        }


        public static class Telegram
        {
            private static int _api_id = Convert.ToInt32(ConfigurationManager.AppSettings["Telegram Api Id"]);
            private static string _api_hash = ConfigurationManager.AppSettings["Telegram Api Hash"];
            private static string _phoneNumber = ConfigurationManager.AppSettings["Telegram PhoneNumber"];
            private static int _teleInterval = Convert.ToInt32(ConfigurationManager.AppSettings["Telegram Timing"]);
            private static bool _newConection = Convert.ToBoolean( ConfigurationManager.AppSettings["Telegram NewConnection"]);

            public static int Api_Id { get => _api_id; }
            public static string Api_hash { get => _api_hash; }
            public static string PhoneNumber { get => _phoneNumber; }
            public static int TeleInterval { get => _teleInterval; }
            public static bool NewConnection { get => _newConection; }

        }

        public static class DataBase
        {
            private static DBProperties _dbProp = new DBProperties(ConfigurationManager.AppSettings["DataBase Source"], ConfigurationManager.AppSettings["DataBase InitialCatalog"]);

            public static DBProperties DBProperties { get => _dbProp; }
        }




    }

   

}
