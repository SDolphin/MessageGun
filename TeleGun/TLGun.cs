using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.IO;

using TLSharp.Core;
using TeleSharp.TL;
using TeleSharp.TL.Contacts;

using Telegram.TeleGun.Common;


using MessageGun.DomainModel.Tools.Log;
using Microsoft.Extensions.Logging;

using MessageGun.DomainModel.Tools.Messaging;

namespace Telegram.TeleGun
{

    /// <summary>
    ///  Simple library for telegram messaging
    ///  Just connect and send. PROFIT
    /// </summary>
    public class TLGun : IDisposable
    {
        private ILogger log = LogManager.GetLogger(typeof(TLGun));


        private const string _sessionFile = "session.dat";

        private int _apiId;
        private string _apiHash;
        private string _phone;


        private TelegramClient client;
        private TLContacts tLContacts;
        private string _tPassword;

        //private static string _telegramServer = "149.154.175.100";
        private static string _telegramServer = "149.154.167.40";
        private static int _telegramPort = 443;

        public static string TelegramServer
        {
            get => _telegramServer;
            set => _telegramServer = value;
        }

        public static int TelegramPort
        {
            get => _telegramPort;
            set => _telegramPort = value;
        }


        public delegate void ReturnPassEventHandler(object obj, PasswordEventArgs eventArgs);
        public event ReturnPassEventHandler PassReturnEvent;
        ManualResetEvent manualPasswordEvent;

        private bool _newSession;


        /// <summary>
        /// Constructor, create connection to telegram server
        /// </summary>
        /// <param name="apiId"></param>
        /// <param name="apiHash"></param>
        /// <param name="newSession"> true - delete session.dat and create new connection with two-factor authorization </param>
        public TLGun(int apiId, string apiHash, bool newSession = false)
        {
            _apiId = apiId;
            _apiHash = apiHash;
            _newSession = newSession;


            Session.ConnectionAdress = _telegramServer;
            Session.ConnectionPort = _telegramPort;

            if (newSession == true)
            {
                if (File.Exists(_sessionFile))
                {
                    File.Delete(_sessionFile);
                }
                
            }
            client = new TelegramClient(apiId, apiHash);
            Thread.Sleep(1000);
            client.ConnectAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Invokes while sennding message, if telegram server forcibly disconnected the connection
        /// </summary>
        public void Reconnect()
        {
            try
            {
                client = new TelegramClient(_apiId, _apiHash);
                client.ConnectAsync().GetAwaiter().GetResult();
                tLContacts = client.GetContactsAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                log.LogError("Reconnect : " + ex.Message);
            }
        }


        /// <summary>
        /// Authorise by our phone number, if in consturctor we set new session connection, 
        /// will invokes second part of two-factor autorisation (PassReturnEvent).
        /// </summary>
        /// <param name="Phone">Number from wich we will send messages</param>
        /// <returns> 'true' - connection is established and 'false' respectively not</returns>
        public bool Connect(string Phone)
        {

            if (_newSession)
            {
                manualPasswordEvent = new ManualResetEvent(false);
                var hash = client.SendCodeRequestAsync(Phone).GetAwaiter().GetResult();

                OnPassReturnEvent(new PasswordEventArgs());
                manualPasswordEvent.WaitOne();

                var user = client.MakeAuthAsync(Phone, hash, _tPassword).GetAwaiter().GetResult();
            }

            try
            {
                tLContacts = client.GetContactsAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                log.LogError("Error While getiing tlContacts", ex.Message);
                return false;
            }

            if (tLContacts == null)
            {
                return false;
            }
            else
            {
                return true;
            }
         
        }

        
        protected void OnPassReturnEvent(PasswordEventArgs eventArgs)
        {
            if (PassReturnEvent != null)
            {
                PassReturnEvent(this, eventArgs);
                _tPassword = eventArgs.Password;
                manualPasswordEvent.Set();
            }
        }


        /// <summary>
        /// Easy send message
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public TLAbsUpdates SendMessageTo(string phone, string message)
        {
            return Send(phone, message);
        }

        /// <summary>
        /// Easy send message
        /// </summary>
        /// <param name="tMessage"></param>
        /// <returns></returns>
        public TLAbsUpdates SendMessageTo(IPhoneInfo tMessage)
        {
            string phone = tMessage.PhoneNumber;
            string message = tMessage.Message;

            return Send(phone, message);
        }


        /// <summary>
        /// find contact in 'our contacts' and if there is one we send message
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private TLAbsUpdates Send(string phone, string message)
        {
            if (tLContacts != null)
            {
                var cuser = tLContacts.Users
                  .OfType<TLUser>()
                  .FirstOrDefault(x => x.Phone == phone);

                if (cuser != null)
                {
                    //try
                    //{
                       return client.SendMessageAsync(new TLInputPeerUser() { UserId = cuser.Id }, message).GetAwaiter().GetResult();
                    //}
                    //catch (Exception ex)
                    //{
                    //    log.LogInformation("Message did not send, reconnection start");
                    //    Reconnect();
                    //    log.LogInformation("Reconnection Complite");
                    //    return client.SendMessageAsync(new TLInputPeerUser() { UserId = cuser.Id }, message).GetAwaiter().GetResult();
                    //}
                }
                else
                {
                    throw new Exception("cuser is null");
                }

            }
            else
            {
                throw new Exception("tLContacts is null");
            }

        }

        public void Dispose()
        {
            client.Dispose();
        }

    }
}
