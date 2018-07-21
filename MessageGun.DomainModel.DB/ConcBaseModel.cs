using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageGun.DomainModel.DB
{
    //class ConcBaseModel
    //{

    //    private SqlConnection sqlConnection;



    //    private TimerCallback GetMessagesFromQueueCallBack;
    //    private Timer timerGetMessagesFromQueue;


    //    public DataBaseModel(DBProperties dBProperties, int miliseconds = 1000)
    //    {

    //        string connoectionString = $"Data Source={dBProperties.DataSource};" +
    //                                   $"Initial Catalog={dBProperties.InitialCatalog};" +
    //                                  "Integrated Security=SSPI;";

    //        sqlConnection = new SqlConnection(connoectionString);

    //        sqlConnection.Open();


    //        SetUpTimeQueueCallbacks(miliseconds);

    //    }

    //    private void SetUpTimeQueueCallbacks(int miliseconds)
    //    {
    //        GetMessagesFromQueueCallBack = new TimerCallback(Dequeuing);
    //        timerGetMessagesFromQueue = new Timer(GetMessagesFromQueueCallBack, null, 0, miliseconds);
    //    }


    //    private void Dequeuing(object obj)
    //    {
    //        IMQuery mQuery;
    //        bool rez = concurrentSendingQueue.TryDequeue(out mQuery);
    //        if (rez == true)
    //        {
    //            try
    //            {
    //                var rezz = InsertIntoMqMessages(mQuery);
    //                incomingDictionary.TryAdd(,)
    //            }
    //            catch (Exception ex)
    //            {
    //                //log.LogError("Message not sended :", ex);
    //            }
    //        }
    //        else
    //        {
    //            // log.LogWarning("Dequeiuing returned false");
    //        }
    //    }

    //    public Dictionary<int, string> GetListOfAvaliablePhones()
    //    {
    //        Dictionary<int, string> dictionary = new Dictionary<int, string>();
    //        SqlCommand command = new SqlCommand(TableExtra.SELECT.GenerateUsersSelect(), sqlConnection);

    //        using (SqlDataReader reader = command.ExecuteReader())
    //        {
    //            while (reader.Read())
    //            {
    //                dictionary.Add((int)reader[TableExtra.UsersPhones.Id]
    //                              , (string)reader[TableExtra.UsersPhones.PhoneNumber]);
    //            }
    //        }
    //        return dictionary;

    //    }


    //    private int InsertIntoMqMessages(IMQuery iMQuery, bool ifSended)
    //    {


    //    }

    //    public ConcurrentQueue<IMQuery> concurrentSendingQueue = new ConcurrentQueue<IMQuery>();
    //    public ConcurrentDictionary<int, IMQuery> incomingDictionary = new ConcurrentDictionary<int, IMQuery>();
    //    public ConcurrentDictionary<ITQuery> concurrentSendingTeleQueue = new ConcurrentQueue<ITQuery>();


    //    private int InsertIntoMqMessages(IMQuery iMQuery)
    //    {

    //        int msId = 0;
    //        using (SqlCommand cmd = new SqlCommand(TableExtra.INSERT.GenerateMqMessageInsert(iMQuery.Time,
    //            iMQuery.Phone, iMQuery.Header, iMQuery.Body), sqlConnection))
    //        {
    //            msId = (int)cmd.ExecuteScalar();
    //        }
    //        return msId;
    //    }




    //    public void Dispose()
    //    {
    //        sqlConnection.Close();
    //    }
    //}
}
