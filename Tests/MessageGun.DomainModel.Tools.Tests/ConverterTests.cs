using System;
using MessageGun.DomainModel.Tools.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;

using System.Xml.Serialization;
using System.IO;

namespace MessageGun.DomainModel.Tools.Tests
{
    [TestClass]
    public class ConverterTests
    {

        private string message = "<NS1:NotificationRq xmlns:NS1=\"http://croc.ru/cppk/esb/messages\"><NS1:RqUID>uid</NS1:RqUID>" +
                "<NS1:RqTm>2018-03-27T11:49:16</NS1:RqTm>" +
                "<NS1:SourceSystem>ZABBIX</NS1:SourceSystem>" +
                "<NS1:Recipient>" +
                    "<NS1:RecipientType>PHONE</NS1:RecipientType>" +
                    "<NS1:Address>79999999999</NS1:Address>" +
                "</NS1:Recipient>" +
                "<NS1:Message>" +
                    "<NS1:Body lang=\"RU\">Some message</NS1:Body>" +
                "</NS1:Message>" +
                "<NS1:Sync>false</NS1:Sync>" +
                "</NS1:NotificationRq>";

        [TestMethod]
        public void CheckParsingmessageStringByXml()
        {
            MqMessageXmlClass mqMessageXmlClass = MqXmlConverter.Deserialize(message);

            Assert.AreNotEqual(null, mqMessageXmlClass);
            Assert.AreEqual("Some message", mqMessageXmlClass.Message.Body);
        }

        [TestMethod]
        public void CheckParsingmessageStringByJson()
        {

            RegularParser regularParser = new RegularParser();
            IDictionary<string, string> dict = regularParser.Parse(message);
            //Assert.AreNotEqual(null, mqMessageXmlClass);
            Assert.AreEqual("Some message", dict["Body"]);


        }

    }
}
