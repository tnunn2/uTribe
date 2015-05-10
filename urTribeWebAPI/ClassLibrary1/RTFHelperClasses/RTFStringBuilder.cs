using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Configuration;

namespace urTribeWebAPI.Messaging
{
    public class RTFStringBuilder : IRTFStringBuilder
    {
        #region Constants
        private const string TestRole = "testRole";
        private const string GlobalAllow = "RU";
        private const string PrimaryKey = "id";
        private const string PrimaryKeyType = "string";
        private const string SecondaryKey = "timestamp";
        private const string SecondaryKeyType = "string";

        private const long OneYearInMilliseconds = 31536000000;
        private const int TablePLoad = 2;
        private const int TablePType = 5;
        private const int ReadOps = 2;
        private const int WriteOps = 2;
        #endregion

        #region ReadOnly
        private readonly string AppKey     = ConfigurationManager.AppSettings["RealtimeBrokerAppKey"];
        private readonly string PrivateKey = ConfigurationManager.AppSettings["RealtimeBrokerPrivateKey"];
        private readonly string RTtoken    = ConfigurationManager.AppSettings["RealtimeBrokerRTtoken"];
        private readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();
        #endregion

        #region Member Variables        
        #endregion

        #region Properties
        private JavaScriptSerializer ObjectSerializer
        {
            get
            {
                return _serializer;
            }
        }
        #endregion

        #region Private Method
        private string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
        #endregion

        #region Public Method
        public string MakeCreateString(string tname)
        {
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
                throughput = new Throughput()
                {
                    read = ReadOps,
                    write = WriteOps
                }
            };
            return ObjectSerializer.Serialize(q);
        }
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
            return ObjectSerializer.Serialize(q);
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
            return ObjectSerializer.Serialize(q);
        }
        #endregion
    }
}
