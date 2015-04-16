using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace urTribeWebAPI.Models
{
    public class RTFHelpers
    {
        //Things we can't change
        private const string RTtoken = "babfbf3c-ba02-11e4-8dfc-aa07a5b093db";
        private const string AppKey = "kSVcgZ";
        private const string PrivateKey = "bMsGyhCfuR0I";
        private const string TypeJson = "application/json; charset=UTF-8";

        //Things we can
        private const string TestRole = "testRole";
        private const string GlobalAllow = "RU";
        private const string PrimaryKey = "id";
        private const string PrimaryKeyType = "string";
        private const string SecondaryKey = "timestamp";
        private const string SecondaryKeyType = "string";

        //The wisdom to know the difference
        private const long OneYearInMilliseconds = 31536000000;
        private const int TablePLoad = 2;
        private const int TablePType = 1;
        //Helper methods section
        /* Spaces had to be removed
             * string data = "{\"applicationKey\": \"" + AppKey + "\", ";
            data = data + "\"authenticationToken\": \"" + RTtoken + "\", ";
            data = data + "\"table\": \"" + tableName + "\", ";
            data = data + "\"key\": { \"primary\": { \"name\": \"id\", \"dataType\": \"string\"},";
            data = data + "\"secondary\": { \"name\": \"timestamp\", \"dataType\": \"string\"}},";
            data = data + "\"provisionType\":1, \"provisionLoad\": 2}"; */
        public static string MakeCreateString(string tname)
        {
            var serializer = new JavaScriptSerializer();
            //Key p = new Key() { name = PrimaryKey, dataType = PrimaryKeyType };
            //Key s = new Key() { name = SecondaryKey, dataType = SecondaryKeyType };
            //TableSchema schema = new TableSchema() { primary = p, secondary = s };
            CreateTableQuery q = new CreateTableQuery()
            {
                applicationKey = AppKey,
                authenticationToken = RTtoken,
                table = tname,
                key = new TableSchema()
                {
                    primary = new Key()
                    {
                        name = PrimaryKey,
                        dataType = PrimaryKeyType
                    },
                    secondary = new Key()
                    {
                        name = SecondaryKey,
                        dataType = SecondaryKeyType
                    }
                },
                provisionLoad = TablePLoad,
                provisionType = TablePType
            };
            return serializer.Serialize(q);
        }

        /*
         * string data = "{\"applicationKey\": \"" + AppKey + "\", ";
            data = data + "\"privateKey\": \"" + PrivateKey + "\", ";
            data = data + "\"authenticationToken\": \"" + userToken + "\", ";
            data = data + "\"roles\": [\"" + TestRole + "\"],";
            data = data + "\"timeout\": " + OneYearInMilliseconds + ", ";
            data = data + "\"policies\": {\"database\": {\"listTables\": [], \"deleteTable\": [], \"createTable\": false, \"updateTable\": [] }, ";
            data = data + "\"tables\": { \"";
            data = data + tableName + "\": { \"allow\":\"RU\" }";
            data = data + "}}}"; 
         */
        public static string MakeAuthString(List<string> tableNames, string userToken)
        {
            Policy p = new Policy()
            {
                database = new DBPolicy()
                {
                    listTables = new List<string>(),
                    deleteTable = new List<string>(),
                    createTable = false,
                    updateTable = new List<string>()
                },
                tables = new Dictionary<string, TablePolicy>()
            };
            foreach (string t in tableNames)
            {
                p.tables.Add(t, new TablePolicy() { allow = GlobalAllow });
            }
            AuthQuery q = new AuthQuery()
            {
                applicationKey = AppKey,
                privateKey = PrivateKey,
                authenticationToken = userToken,
                roles = new List<string> { TestRole },
                timeout = OneYearInMilliseconds,
                policies = p
            };
            return JsonConvert.SerializeObject(q);
        }

        public static string MakeInviteString(string userToken, string userTableName, string eventTableName)
        {
            PutItemQuery q = new PutItemQuery()
            {
                applicationKey = AppKey,
                privateKey = PrivateKey,
                authenticationToken = userToken,
                table = userTableName,
                item = new Dictionary<string, string> { { PrimaryKey, eventTableName } }
            };
            return JsonConvert.SerializeObject(q);
        }

        public static string SendRequest(string url, string data)
        {
            WebRequest myRequest = WebRequest.Create(url);
            myRequest.Method = "PUT";
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


        public static string eidToEtableName(Guid eventID)
        {
            string tableName = "event" + eventID;
            return tableName;
        }
    }
}