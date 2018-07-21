using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageGun.DomainModel.DB.Common.Interfaces;
//using MessageGun.DomainModel.DB.EntityBd;
using MessageGun.DomainModel.DB.Common;
using Microsoft.Extensions.Logging;
using MessageGun.DomainModel.Tools.Log;

namespace MessageGun.DomainModel.DB
{
    public class EntityDataBaseModel
    {

        private ILogger log = LogManager.GetLogger(typeof(EntityDataBaseModel));

        string _conectionString;

        

        public EntityDataBaseModel(DBProperties dBProperties)
        {
            _conectionString = SetupConnections( dBProperties.DataSource , dBProperties.InitialCatalog );

        }

        public string SetupConnections(string serverName, string databaseName)
        {

            //Specify the provider name, server and database.
            string providerName = "System.Data.SqlClient";

            // Initialize the connection string builder for the
            // underlying provider.
            SqlConnectionStringBuilder sqlBuilder =
                new SqlConnectionStringBuilder();


            sqlBuilder.DataSource = serverName;
            sqlBuilder.InitialCatalog = databaseName;
            sqlBuilder.IntegratedSecurity = false;
            //sqlBuilder.UserID = "USERNAME";
            //sqlBuilder.Password = "PASS";

            // Build the SqlConnection connection string.
            string providerString = sqlBuilder.ToString();

            // Initialize the EntityConnectionStringBuilder.
            EntityConnectionStringBuilder entityBuilder =
                new EntityConnectionStringBuilder();

            //Set the provider name.
            entityBuilder.Provider = providerName;

            // Set the provider-specific connection string.
            entityBuilder.ProviderConnectionString = providerString;

            // Set the Metadata location.
            entityBuilder.Metadata = @"res://*/EntityMGDataBase.csdl|
                                       res://*/EntityMGDataBase.ssdl|
                                       res://*/EntityMGDataBase.msl";

            using (EntityConnection conn =
                new EntityConnection(entityBuilder.ToString()))
            {
                conn.Open();
                
                conn.Close();
            }
            return entityBuilder.ToString();
        }


        public Dictionary<int, string> GetListOfAvaliablePhones()
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();

            using (Entities entities = new Entities(_conectionString))
            {
                foreach (UsersPhones uPhone in entities.UsersPhones)
                {
                    if (uPhone.Avaliable == true)
                    {
                        dictionary.Add(uPhone.Id, uPhone.PhoneNumber);
                    }
                } 
            }
            return dictionary;
        }

        public int InsertIntoMqMessages(IMQuery iMQuery)
        {
            return InsertIntoMqMessages(iMQuery.Time, iMQuery.PhoneNumber, iMQuery.Header, iMQuery.Body);
        }


        public int InsertIntoMqMessages(DateTime time, string phoneNumber, string header, string body)
        {
            MqMessages mqMessages = new MqMessages() {
                Time = time,
                Phone = phoneNumber,
                Header = header,
                Body = body
            };

            int msId = 0;
            using (Entities entities = new Entities(_conectionString))
            {

                entities.MqMessages.Add(mqMessages);
                entities.SaveChanges();
                msId = mqMessages.Id;
            }

            return msId;
        }


        public void InsertIntoTeleMessages(int messageId, bool isSended, string Description = null)
        {
            TeleMessages teleMessages = new TeleMessages()
            {
                id_messages = messageId,
                Sended = isSended,
                Description = Description
            };

            using (Entities entities = new Entities(_conectionString))
            {
                entities.TeleMessages.Add(teleMessages);
                entities.SaveChanges();
            }

        }


    }
}
