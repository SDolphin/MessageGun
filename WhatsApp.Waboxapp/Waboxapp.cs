using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using System.Web;

using WhatsApp.Waboxapp.Common;


using RestSharp;
using RestSharp.Authenticators;
using System.Web.Script.Serialization;

namespace WhatsApp.Waboxapp
{
    public class Waboxapp
    {
        private const string _url = "https://www.waboxapp.com/api/send/chat";

        private WabCon _wabCon;
        WebProxy _proxy;


        public Waboxapp(WabCon wabCon, WebProxy proxy = null)
        {
            _wabCon = wabCon;
            _proxy = proxy;
        }

        private WebProxy Proxy
        {
            get => _proxy;
            set => _proxy = value;
        }

        public HttpStatusCode SendMessage(int id ,string toPhone, string message)
        {

            Hashtable hashtable = PrepareMessage(id, toPhone, message);
            return PostData(hashtable);
        }

        private Hashtable PrepareMessage(int id, string toPhone, string message)
        {
            Hashtable hashtable = BaseValues();
            hashtable.Add("to", toPhone);
            hashtable.Add("custom_uid", id);
            hashtable.Add("text", message);

            return hashtable;
        }


        private Hashtable BaseValues()
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("token", _wabCon.Token);
            hashtable.Add("uid", _wabCon.PhoneFrom);
            return hashtable;
        }

        private HttpStatusCode PostData(Hashtable values)
        {
            string data = PrepareData(values);

            //TODO SET PROXY костыль
            _proxy = new System.Net.WebProxy("host", true);
            _proxy.Credentials = new System.Net.NetworkCredential("LOGIN", "PASSWORD");

            var client = new RestClient(_url);
            var request = new RestRequest(Method.POST);

            if(_proxy!= null) client.Proxy = _proxy;

            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", data, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            return response.StatusCode;

        }

        private string PrepareData(Hashtable values)
        {
            StringBuilder data = new StringBuilder(string.Empty);
            foreach (var key in values.Keys)
            {
                if (data.Length != 0)
                {
                    data.Append('&');
                }
                if (values[key] != null)
                {
                    data.Append(key.ToString());
                    data.Append('=');
                    data.Append(HttpUtility.UrlEncode(values[key].ToString()));
                }
            }
            return data.ToString();
        }


    }
}
