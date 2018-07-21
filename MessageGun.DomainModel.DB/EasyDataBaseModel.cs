using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MessageGun.DomainModel.DB.Common;
using MessageGun.DomainModel.DB.Common.Interfaces;

namespace MessageGun.DomainModel.DB
{
    public class EasyDataBaseModel : IDisposable
    {
        private SqlConnection sqlConnection;

        public EasyDataBaseModel(DBProperties dBProperties )
        {

            string connoectionString = $"Data Source={dBProperties.DataSource};" +
                                       $"Initial Catalog={dBProperties.InitialCatalog};" +
                                      "Integrated Security=SSPI;";

            sqlConnection = new SqlConnection(connoectionString);

            sqlConnection.Open();

        }


        public Dictionary<int, string> GetListOfAvaliablePhones()
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            SqlCommand command = new SqlCommand(TableExtra.SELECT.GenerateUsersSelect(), sqlConnection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    dictionary.Add((int)reader[TableExtra.UsersPhones.Id]
                                  , (string)reader[TableExtra.UsersPhones.PhoneNumber]);
                }
            }
            return dictionary;

        }


        public int InsertIntoMqMessages(IMQuery iMQuery)
        {
            return InsertIntoMqMessages(iMQuery.Time, iMQuery.PhoneNumber, iMQuery.Header , iMQuery.Body);

        }


        public int InsertIntoMqMessages(DateTime time, string phoneNumber, string header, string body)
        {
            int msId = 0;
            using (SqlCommand cmd = new SqlCommand(TableExtra.INSERT.GenerateMqMessageInsert(time,
                phoneNumber, header, body), sqlConnection))
            {
                msId = (int)cmd.ExecuteScalar();
            }
            return msId;
        }


        public void InsertIntoTeleMessages(int messageId , bool isSended, string Description = null)
        {
            using (SqlCommand cmd = new SqlCommand(TableExtra.INSERT.GenerateTeleMessageInsert(messageId, isSended, Description), sqlConnection))
            {
                cmd.ExecuteNonQuery();
            }
        }


        public void Dispose()
        {
            sqlConnection.Close();
        }

    }
}
