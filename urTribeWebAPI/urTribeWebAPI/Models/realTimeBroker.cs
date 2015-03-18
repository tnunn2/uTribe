using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using urTribeWebAPI.Common.Interfaces;

// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace urTribeWebAPI.Models
{
    public class RealtimeBroker : IMessageBroker
    {
        //Things we can't change
        private const string RTtoken = "babfbf3c-ba02-11e4-8dfc-aa07a5b093db";
        private const string AppKey = "kSVcgZ";
        private const string PrivateKey = "bMsGyhCfuR0I";
        private const string TypeJson = "application/json; charset=UTF-8";
        public const string CreateUrl = "https://storage-backend-prd-useast1.realtime.co/createTable";
        private const string AuthUrl = "https://storage-backend-prd-useast1.realtime.co/authenticate";
        private const string putItemURL = "https://storage-backend-prd-useast1.realtime.co/putItem";

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
        private const int TablePType = 5;
        private const int ReadOps = 2;
        private const int WriteOps = 2;


    //Helper methods section
        /* Spaces had to be removed
             * string data = "{\"applicationKey\": \"" + AppKey + "\", ";
            data = data + "\"authenticationToken\": \"" + RTtoken + "\", ";
            data = data + "\"table\": \"" + tableName + "\", ";
            data = data + "\"key\": { \"primary\": { \"name\": \"id\", \"dataType\": \"string\"},";
            data = data + "\"secondary\": { \"name\": \"timestamp\", \"dataType\": \"string\"}},";
            data = data + "\"provisionType\":1, \"provisionLoad\": 2}"; */
        public string MakeCreateString(string tname)
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
                provisionType = TablePType,
                throughput = new Throughput() {
                    read = ReadOps,
                    write = WriteOps
            }
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
        public string MakeAuthString(List<string> tableNames, string userToken)
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

        public string MakeInviteString(string userToken, string userTableName, string eventTableName, string invitedBy)
        {
            PutItemQuery q = new PutItemQuery()
            {
                applicationKey = AppKey,
                privateKey = PrivateKey,
                authenticationToken = userToken,
                table = userTableName,
                item = new Dictionary<string, string>
                {
                    { PrimaryKey, eventTableName },
                    { SecondaryKey, GetTimestamp(DateTime.Now)},
                    {"Invited By", invitedBy}
                }
            };
            return JsonConvert.SerializeObject(q);
        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        public string SendRequest(string url, string data)
        {
            WebRequest myRequest = WebRequest.Create(url);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/json; charset=UTF-8";
            Debug.Print(data);
            Debug.Print(""+ data.Length);

            UTF8Encoding enc = new UTF8Encoding();
            using (Stream ds = myRequest.GetRequestStream())
            {
                ds.Write(enc.GetBytes(data), 0, data.Length);
            }
            WebResponse wr = myRequest.GetResponse();
            Stream receiveStream = wr.GetResponseStream();
            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
            return reader.ReadToEnd();
            //return "";
        }
    
        public string eidToEtableName(string eventID)
        {
            return "event" + eventID;
        }

        public string userToTableName(IUser user)
        {
            return "user" + user.ID;
        }

        //End helper section

        public void CreateAuthAndInvite(string eventID, IUser eventCreator, IEnumerable<IUser> invitees)
        {
            string tableName = eidToEtableName(eventID);
            CreateEventChannel(tableName, eventCreator);
            AuthenticateUser(eventCreator, tableName);
            foreach (IUser invitee in invitees)
            {
                AuthenticateUser(invitee, tableName);
                PutInvite(invitee, tableName, eventCreator.Name);
            }
        }

        public void CreateEventChannel(string tableName, IUser eventCreator)
        {
            string data = MakeCreateString(tableName);

            try {
                string creationResult = SendRequest(CreateUrl, data);
            } catch (WebException e)
                { throw new Exception("Event table creation failed. Code " + e.Message); }
        }

        public void CreateEventChannel(Guid eventID, IUser eventCreator, IEnumerable<IUser> invitees)
        {
            throw new NotImplementedException();
        }

        public string CreateUserChannel(IUser user)
        {
            string tableName = userToTableName(user);
            string data = MakeCreateString(tableName);
            Debug.Write(SendRequest(CreateUrl, data));
            return tableName;
        }

        public void CreateUserChannel(string tableName)
        { 
            string data = MakeCreateString(tableName);
            Debug.Write(SendRequest(CreateUrl, data));
        }

        /*TODO: 
         * 1)DONE
         * 2)write unit tests --sorta done
         * 3)exception handling
         * 4)DONE
         * 5)implement wait for resource busy "creating" state
        */
        private string AuthUser(List<string> tableNames, string userToken)
        {
            //bool busy = true;
            WebRequest myRequest = WebRequest.Create(AuthUrl);
            myRequest.Method = "POST";
            myRequest.ContentType = TypeJson;

            string data = MakeAuthString(tableNames, userToken);
            try {string result = SendRequest(AuthUrl, data);} 
            catch (WebException e)
                { throw new Exception("Authorization failed. Code " + e.Message); }

            return "";
        }

        public void AddToChannel(IUser user, Guid eventId)
        {
            AuthenticateUser(user, eidToEtableName(eventId.ToString()));
        }

        //Current implementation only allows for all users to have the same (RU) policy on table. 
        //Otherwise we have to store each policy in DB.
        public string AuthenticateUser(IUser user, string tableName)
        {
            List<string> tableNames = user.Channels;
            //List<string> tableNames = new List<string>();
            tableNames.Add(tableName);
            string userToken = user.Token;
            try
            {
                string result = AuthUser(tableNames, userToken);
                user.Channels.Add(tableName);
                return tableName;
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }
    

        public bool PutInvite(IUser user, string eventTable, string invitedBy)
        {
            string userToken = user.Token;
            string userTableName = user.InvitesChannel;
            string data = MakeInviteString(userToken, userTableName, eventTable, invitedBy);
            try
            {
                string result = SendRequest(putItemURL, data);
                Debug.Write(result);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        

        

    }
}
