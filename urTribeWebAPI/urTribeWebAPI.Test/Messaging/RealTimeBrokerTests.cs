using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Core;
using urTribeWebAPI.BAL;
using urTribeWebAPI.Common;
using urTribeWebAPI.Messaging;
using urTribeWebAPI.Models;
using Assert = NUnit.Framework.Assert;

// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace urTribeWebAPI.Test.Messaging
{
    [TestClass]
    public class RealTimeBrokerTests
        
    {

        [TestMethod]
        public void TestConfig()
        {
            string AppKey = urTribeWebAPI.Messaging.Properties.Settings.Default.RTFAppKey;
            string PrivateKey = urTribeWebAPI.Messaging.Properties.Settings.Default.RTFPrivateKey;
            string RTtoken = urTribeWebAPI.Messaging.Properties.Settings.Default.RTFToken;
            if (AppKey == null) Assert.Fail("AppKey null");
            if (PrivateKey == null) Assert.Fail("Private Key null");
            if (RTtoken == null) Assert.Fail("RT Token null");
            Assert.AreEqual(urTribeWebAPI.Messaging.Properties.Settings.Default.RTFCreateURL, "https://storage-backend-prd-useast1.realtime.co/createTable");
        }
        
        [TestMethod]
        public void TestCreationString()
        {
            string RTtoken = "8c01b1ae-e87e-11e4-a50b-99b8eb024984";
            string AppKey = "rUaRaB";
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

            IRTFStringBuilder sb = new RTFStringBuilder();
            string data2 = sb.MakeCreateString(tableName);
            Debug.Print(data);
            Assert.AreEqual(data, data2);
        }

        [TestMethod]
        public void TestAuthString()
        {
            string AppKey = "rUaRaB";
            string PKey = "O4h7dcnRUIIL";
            // ReSharper disable once ConvertToConstant.Local
            string tableName = "test001";
            string userToken = "testToken";
            string TestRole = "TestRole";
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
            IRTFStringBuilder sb = new RTFStringBuilder();
            string data2 = sb.MakeAuthString(names, userToken);
            Debug.Print(data);
            Assert.AreEqual(data, data2);
        }

        /*[TestMethod]
        public void TestActualCreation()
        {
            Guid eventID = new Guid("7c74ac99-4cdf-4f3a-a63b-fc040f300607");
            string eventTable = "event" + eventID;
            RealTimeBroker b = new RealTimeBroker();
                
            IUser creator = new User()
            {
                ID = new Guid("aa918dde-94e0-4323-a281-c8274d67eaca"),
                AuthenticatedChannels = new List<string>() { eventTable},
                Name = "Catherine C"
            };

            Debug.Print(b.JustCreateChannel(eventTable));
        }

        [TestMethod]
        public void TestNewUserRegistration()
        {

            Guid eventID = new Guid("7c74ac99-4cdf-4f3a-a63b-fc040f300607");
            string eventTable = "event" + eventID;
            UserFacade f = new UserFacade();
            IUser creator = new User()
            {
                ID = new Guid("aa918dde-94e0-4323-a281-c8274d67eaca"),
                AuthenticatedChannels = new List<string>(),
                Name = "Catherine C"
            };
            string table = f.registerNewUserWithRTF(creator);
            Assert.AreEqual(table, "user"+creator.ID);
        }
        
        [TestMethod]
        public void TestPresentationEvent()
        {
            try
            {
                int numInvitees = 1;
                Guid eventID = new Guid("7c74ac99-4cdf-4f3a-a63b-fc040f300607");
                string eventTable = "event" + eventID;
                RealTimeBroker b = new RealTimeBroker();
                
                IUser creator = new User()
                {
                    ID = new Guid("aa918dde-94e0-4323-a281-c8274d67eaca"),
                    AuthenticatedChannels = new List<string>() { eventTable},
                    Name = "Catherine C"
                };
                
                List<IUser> invitees = new List<IUser>();
                invitees.Add(new User
                    {
                        Name = "Benjamin D",
                        ID = new Guid("b40354dc-5734-432e-b6c5-24adf8890312"),
                        AuthenticatedChannels = new List<string> { eventTable}
                    });
                invitees.ForEach(u => u.UserChannel = b.CreateUserChannel(u));
                b.CreateAuthAndInvite(eventID, creator, invitees);
                
                b.InviteUsers(invitees, creator.Name, eventTable);

            }
            catch (Exception e)
            {
               Debug.Print(e.Message);
               Assert.Fail();
            }
        } */
        //mks was here
        /*[TestMethod]
        public void testPutItem()
        {
            IUser u = new User()
            {
                ID = new Guid("d19fc3d0-f7d0-4f4c-b0e4-5681afb0bde9"),
                UserChannel = "userd19fc3d0-f7d0-4f4c-b0e4-5681afb0bde9",
                AuthenticatedChannels = new List<string>
                {
                    "userd19fc3d0-f7d0-4f4c-b0e4-5681afb0bde9",
                    "event1"
                }
            };
            RealtimeBroker b = new RealtimeBroker();
            b.AuthenticateUser(u, u.UserChannel);
            b.PutInvite(u, "event1");

        }*/
    }
}
