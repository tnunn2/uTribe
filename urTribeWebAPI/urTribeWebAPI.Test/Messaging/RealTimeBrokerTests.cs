using System;
using System.Web.Script.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using urTribeWebAPI.Models;
using Assert = NUnit.Framework.Assert;
// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace urTribeWebAPI.Test.Messaging
{
    [TestClass]
    public class RealTimeBrokerTests
        
    {
        [TestMethod]
        public void TestCreateChannel()
        {
            string RTtoken = "babfbf3c-ba02-11e4-8dfc-aa07a5b093db";
            string AppKey = "kSVcgZ";
            string PKey = "bMsGyhCfuR0I";
            string CreateUrl = "https://storage-backend-prd-useast1.realtime.co/createTable";
            // ReSharper disable once ConvertToConstant.Local
            string tableName = "test001";
            string data = "{\"applicationKey\":\"" + AppKey + "\",";
            data = data + "\"authenticationToken\":\"" + RTtoken + "\",";
            data = data + "\"table\":\"" + tableName + "\",";
            data = data + "\"key\":{\"primary\":{\"name\":\"id\",\"dataType\":\"string\"},";
            data = data + "\"secondary\":{\"name\":\"timestamp\",\"dataType\":\"string\"}},";
            data = data + "\"provisionType\":1,\"provisionLoad\":2}";

            var serializer = new JavaScriptSerializer();
            Key p = new Key() { name = "id", dataType = "string" };
            Key s = new Key() { name = "timestamp", dataType = "string" };
            TableSchema schema = new TableSchema() { primary = p, secondary = s };
            CreateTableQuery q = new CreateTableQuery()
            {
                applicationKey = AppKey,
                authenticationToken = RTtoken,
                table = tableName,
                key = schema,
                provisionType = 1,
                provisionLoad = 2,
            };
            string data2 = serializer.Serialize(q);
            Assert.AreEqual(data, data2);
        }
    }
}
