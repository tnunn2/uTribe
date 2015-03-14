using System;
using System.Web.Script.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using urTribeWebAPI.Models;
using Assert = NUnit.Framework.Assert;
using System.Collections.Generic;
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
            // ReSharper disable once ConvertToConstant.Local
            string tableName = "test001";
            string data = "{\"applicationKey\":\"" + AppKey + "\",";
            data = data + "\"authenticationToken\":\"" + RTtoken + "\",";
            data = data + "\"table\":\"" + tableName + "\",";
            data = data + "\"key\":{\"primary\":{\"name\":\"id\",\"dataType\":\"string\"},";
            data = data + "\"secondary\":{\"name\":\"timestamp\",\"dataType\":\"string\"}},";
            data = data + "\"provisionType\":1,\"provisionLoad\":2}";

            RealtimeBroker b = new RealtimeBroker();
            string data2 = b.MakeCreateString(tableName);
            Assert.AreEqual(data, data2);
        }

        [TestMethod]
        public void TestAuthUser()
        {
            string AppKey = "kSVcgZ";
            string PKey = "bMsGyhCfuR0I";
            // ReSharper disable once ConvertToConstant.Local
            string tableName = "test001";
            string userToken = "testToken";
            string TestRole = "testRole";
            long OneYearInMilliseconds = 31536000000;

            string data = "{\"applicationKey\":\"" + AppKey + "\",";
            data = data + "\"privateKey\":\"" + PKey + "\",";
            data = data + "\"authenticationToken\":\"" + userToken + "\",";
            data = data + "\"roles\":[\"" + TestRole + "\"],";
            data = data + "\"timeout\":" + OneYearInMilliseconds + ",";
            data = data + "\"policies\":{\"database\":{\"listTables\":[],\"deleteTable\":[],\"createTable\":false,\"updateTable\":[]},";
            data = data + "\"tables\":{\"";
            data = data + tableName + "\":{\"allow\":\"RU\"}";
            data = data + "}}}";

            List<string> names = new List<string> {tableName};
            RealtimeBroker b = new RealtimeBroker();
            string data2 = b.MakeAuthString(names, userToken);
            Assert.AreEqual(data, data2);
        }
    }
}
