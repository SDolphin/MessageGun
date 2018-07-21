using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageGun.DomainModel.DB.Common
{
    public static class TableExtra
    {

        #region Users phone table
        private static string UserPhonesTable { get => "UsersPhones"; }
        public static class UsersPhones
        {
            public static string Id { get => "Id"; }
            public static string FirstName { get => "FirstName"; }
            public static string LastName { get => "LastName"; }
            public static string PhoneNumber { get => "PhoneNumber"; }

        }

        public static class SELECT
        {
            public static string GenerateUsersSelect()
            {
                return string.Format($"SELECT {UsersPhones.Id} , {UsersPhones.PhoneNumber} FROM [dbo].[{UserPhonesTable}]");
            }
        }

        #endregion


        private static string MqMessagesTable { get => "MqMessages"; }
        public static class MqMessages
        {            
            public static string Time { get => "Time"; }
            public static string Phone { get => "Phone"; }
            public static string Header { get => "Header"; }
            public static string Body { get => "Body"; }
        }

        private static string TeleMessagesTable { get => "TeleMessages"; }
        public static class TeleMessages
        {
            public static string Id_message { get => "id_messages"; }
            public static string Sended { get => "Sended"; }
            public static string Description { get => "Description"; }
        }



        public static class INSERT
        {
            public static string GenerateMqMessageInsert(DateTime time, string phone, string header, string body)
            {

                string dbFormatTime = time.ToString("yyyy-MM-dd hh:mm:ss");

                
                return string.Format($"INSERT INTO {MqMessagesTable}(" +
                    $"{MqMessages.Time} , " +
                    $"{MqMessages.Phone} , " +
                    $"{MqMessages.Header} , " +
                    $"{MqMessages.Body}) " +
                    $" output INSERTED.ID VALUES( " +
                    $" '{dbFormatTime}' , " +
                    $" '{phone}' , " +
                    $" N'{header}' , " +
                    $" N'{body}'  );");
            }


            public static string GenerateTeleMessageInsert(int id, bool sended, string description)
            {
                byte bit;
                if (sended)
                    bit = 1;
                else
                    bit = 0;

                return string.Format($"INSERT INTO {TeleMessagesTable}(" +
                    $" {TeleMessages.Id_message} , " +
                    $" {TeleMessages.Sended} , " +
                    $" {TeleMessages.Description}) " +
                    $" VALUES(" +
                    $" {id} , " +
                    $" {bit} , " +
                    $"  N'{description}' );");
            }
        }


    }
}
