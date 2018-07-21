using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBM.WMQ;

namespace MQ.MClientModel.Common
{
    public class MQProperties : IQProperties
    {
        private string _queueManagerName;
        private string _host;
        private int _port;
        private string _chanel;
        private string _username;
        private string _password;
        //private string _queueName;

       /* public MQProperties(IQProperties qProperties)
        {
            _queueManagerName = qProperties.QueueManagerName;
            _host = qProperties.Host;
            _port
        }*/

        public string QueueManagerName
        {
            get => _queueManagerName;
            set => _queueManagerName = value;
        }

        public string Host
        {
            get => _host;
            set => _host = value;
        }

        public int Port
        {
            get => _port;
            set => _port = value;
        }

        public string Chanel
        {
            get => _chanel;
            set => _chanel = value;
        }

        public string Username
        {
            get => _username;
            set => _username = value;
        }

        public string Password
        {
            get => _password;
            set => _password = value;
        }

        //public string QueueName
        //{
        //    get => _queueName;
        //    set => _queueName = value;
        //}

        public Hashtable GetQueueConnectionProperties()
        {
            Hashtable queuePropertie = new Hashtable();
            SafeAddQueueProp(ref queuePropertie, MQC.HOST_NAME_PROPERTY, _host);
            SafeAddQueueProp(ref queuePropertie, MQC.PORT_PROPERTY, _port);
            SafeAddQueueProp(ref queuePropertie, MQC.CHANNEL_PROPERTY, _chanel);

            if (!string.IsNullOrEmpty(_password))
                queuePropertie[MQC.USER_ID_PROPERTY] = _username;

            if (!string.IsNullOrEmpty(_password))
                queuePropertie[MQC.PASSWORD_PROPERTY] = _password;



            return queuePropertie;

        }

        private void SafeAddQueueProp(ref Hashtable queuePropertie, string key,  object obj)
        {
            if (obj == null)
            {
                throw new PropertiesException("Обязателный объект настроек равен null");
            }
            else if (obj is string)
            {
                if (string.IsNullOrEmpty(obj as string))
                {
                    throw new PropertiesException("Обязателный объект настроек равен пустой строке");
                }
                else
                {
                    queuePropertie[key] = obj;
                }
            }
            else if (obj is int)
            {
                if ((int)obj == 0)
                {
                    throw new PropertiesException("Обязателный объект настроек равен 0");
                }
                else
                {
                    queuePropertie[key] = obj;
                }
            }
        }



    }
}
