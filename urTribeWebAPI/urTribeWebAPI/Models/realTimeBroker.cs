using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace urTribeWebAPI.Models
{
    public class RealtimeBroker : IMessageBroker
    {
        private const string RTtoken = "babfbf3c-ba02-11e4-8dfc-aa07a5b093db";
        private const string AppKey = "kSVcgZ";
        private const string PKey = "bMsGyhCfuR0I";
        private const string CreateUrl = "https://storage-backend-prd-useast1.realtime.co/createTable";
        private const string authURL = "https://storage-backend-prd-useast1.realtime.co/authenticate";
        private const string TestRole = "testRole";
        private const long OneYearInMilliseconds = 31536000000;

        public void CreateChannel(Guid eventID, User eventCreator, List<User> invitees)
        {
            
            string tableName = "event" + eventID.ToString();
            WebRequest myRequest = WebRequest.Create(CreateUrl);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/json; charset=UTF-8";
            string data = "{\"applicationKey\": \"" + AppKey + "\", ";
            data = data + "\"authenticationToken\": \"" + RTtoken + "\", ";
            data = data + "\"table\": \"" + tableName + "\", ";
            data = data + "\"key\": { \"primary\": { \"name\": \"id\", \"dataType\": \"string\"},";
            data = data + "\"secondary\": { \"name\": \"timestamp\", \"dataType\": \"string\"}},";
            data = data + "\"provisionType\":1, \"provisionLoad\": 2}";

            myRequest.ContentLength = data.Length;
            UTF8Encoding enc = new UTF8Encoding();
            using (Stream ds = myRequest.GetRequestStream())
            {
                ds.Write(enc.GetBytes(data), 0, data.Length);
            }
            WebResponse wr = myRequest.GetResponse();
            Stream receiveStream = wr.GetResponseStream();
            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
            string content = reader.ReadToEnd();
            Console.WriteLine(content);

            string userToken = Guid.NewGuid().ToString();
            authUser(tableName, userToken);
        }

        static void authUser(String tableName, String userToken)
        {
            bool busy = true;


            WebRequest myRequest = WebRequest.Create(authURL);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/json; charset=UTF-8";
            string data = "{\"applicationKey\": \"" + AppKey + "\", ";
            data = data + "\"privateKey\": \"" + PKey + "\", ";
            data = data + "\"authenticationToken\": \"" + userToken + "\", ";
            data = data + "\"roles\": [\"" + TestRole + "\"],";
            data = data + "\"timeout\": " + OneYearInMilliseconds + ", ";
            data = data + "\"policies\": {\"database\": {\"listTables\": [], \"deleteTable\": [], \"createTable\": false, \"updateTable\": [] }, ";
            data = data + "\"tables\": { \"";
            data = data + tableName + "\": { \"allow\":\"RU\" }";
            data = data + "}}}";

            UTF8Encoding enc = new UTF8Encoding();
            using (Stream ds = myRequest.GetRequestStream())
            {
                ds.Write(enc.GetBytes(data), 0, data.Length);
            }
            //while (busy)
            {
                WebResponse wr = myRequest.GetResponse();
                Stream receiveStream = wr.GetResponseStream();
                StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
                string content = reader.ReadToEnd();

            }
        }

        public void AddToChannel(User inviteeGuid, int eventId)
        {
            throw new NotImplementedException();
        }

        private string AuthenticateUser(string Username)
        {
            return "fake-authenticated!";
        }

        private string createMessageTable(Guid EventID)
        {

            return "";
        }

        private string getTablesForUser(User creator)
        {
            return "";
        }
    }
}