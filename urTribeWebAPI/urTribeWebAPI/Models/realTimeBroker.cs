using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace urTribeWebAPI.Models
{
    public class RealtimeBroker : IMessageBroker
    {
        private readonly static string RTtoken = "babfbf3c-ba02-11e4-8dfc-aa07a5b093db";
        private readonly static string appKey = "kSVcgZ";
        private readonly static string pKey = "bMsGyhCfuR0I";

        public void CreateChannel(Guid eventID, User eventCreator, List<User> invitees)
        {

            
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