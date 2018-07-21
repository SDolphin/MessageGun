using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MessageGun.DomainModel.Tools.Converters
{
    public static class MqXmlConverter
    {

        public static MqMessageXmlClass Deserialize(string message)
        {
            MqMessageXmlClass mqMessageXmlClass = new MqMessageXmlClass();
            XmlSerializer serializer = new XmlSerializer(typeof(MqMessageXmlClass));

            try
            {
                return mqMessageXmlClass = (MqMessageXmlClass)serializer.Deserialize(new StringReader(message));
            }
            catch (Exception ex)
            {
                return null;
            }


        }

    }


}