using System;
using System.Web.Script.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using urTribeWebAPI.Models;
using Assert = NUnit.Framework.Assert;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using urTribeWebAPI.Common.Concrete;
using urTribeWebAPI.Common.Interfaces;

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
            const int readOps = 2;
            const int writeOps = 2;
            // ReSharper disable once ConvertToConstant.Local
            string tableName = "test001";
            string data = "{\"applicationKey\":\"" + AppKey + "\",";
            data = data + "\"authenticationToken\":\"" + RTtoken + "\",";
            data = data + "\"table\":\"" + tableName + "\",";
            data = data + "\"key\":{\"primary\":{\"name\":\"id\",\"dataType\":\"string\"},";
            data = data + "\"secondary\":{\"name\":\"timestamp\",\"dataType\":\"string\"}},";
            data = data + "\"provisionType\":5,\"provisionLoad\":2,";
            data = data + "\"throughput\":{\"read\":"+readOps+",\"write\":"+writeOps+"}";
            data = data + "}";

            RealtimeBroker b = new RealtimeBroker();
            string data2 = RTFHelpers.MakeCreateString(tableName);
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
            string data2 = RTFHelpers.MakeAuthString(names, userToken);
            Assert.AreEqual(data, data2);
        }

        [TestMethod]
        public void TestPresentationEvent()
        {
            try
            {
                int numInvitees = 1;
                string eventTable = "event2";
                RealtimeBroker b = new RealtimeBroker();
                IUser creator = new User()
                {
                    ID = Guid.NewGuid(),
                    Channels = new List<string>() { eventTable},
                    Name = "Korra"
                };
                b.CreateEventChannel(eventTable, creator);
                List<IUser> invitees = new List<IUser>();
                for (int i = 0; i < numInvitees; i++)
                {
                    invitees.Add(new User
                    {
                        ID = Guid.NewGuid(),
                        Channels = new List<string> { eventTable}
                    });
                }
                invitees.Add(creator);
                invitees.ForEach(u => u.InvitesChannel = b.CreateUserChannel(u));
                Thread.Sleep(20000);
                invitees.ForEach(u =>
                {
                    b.AuthenticateUser(u, u.InvitesChannel);
                    b.PutInvite(u, eventTable, creator.Name);
                });
                
            }
            catch (Exception e)
            {
               Debug.Print(e.Message);
               Assert.Fail();
            }
        }

        /*[TestMethod]
        public void testPutItem()
        {
            IUser u = new User()
            {
                ID = new Guid("d19fc3d0-f7d0-4f4c-b0e4-5681afb0bde9"),
                InvitesChannel = "userd19fc3d0-f7d0-4f4c-b0e4-5681afb0bde9",
                Channels = new List<string>
                {
                    "userd19fc3d0-f7d0-4f4c-b0e4-5681afb0bde9",
                    "event1"
                }
            };
            RealtimeBroker b = new RealtimeBroker();
            b.AuthenticateUser(u, u.InvitesChannel);
            b.PutInvite(u, "event1");

        }*/
    }
}
