using System.IO;
using System.Net;
using System.Text;

namespace urTribeWebAPI.Messaging
{
    public class RealTimeFrameworkConnect : IMessageConnect
    {
        #region Constants
        private const string TypeJson = "application/json; charset=UTF-8";
        #endregion

        #region Public Method
        public string SendRequest(string url, string data)
        {
            WebRequest myRequest = WebRequest.Create(url);
            myRequest.Method = "POST";
            myRequest.ContentType = TypeJson;

            UTF8Encoding enc = new UTF8Encoding();
            using (Stream ds = myRequest.GetRequestStream())
            {
                ds.Write(enc.GetBytes(data), 0, data.Length);
            }
            WebResponse wr = myRequest.GetResponse();
            Stream receiveStream = wr.GetResponseStream();
            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        public int CreationSleepTime()
        {
            return Properties.Settings.Default.RTFCreationSleepTime;
        }

        #endregion
    }
}
