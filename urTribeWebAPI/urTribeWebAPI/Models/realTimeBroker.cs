using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using urTribeWebAPI.Common.Interfaces;

// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace urTribeWebAPI.Models
{
    public class RealtimeBroker : IMessageBroker
    {
        //Things we can't change

        private const string CreateUrl = "https://storage-backend-prd-useast1.realtime.co/createTable";
        private const string AuthUrl = "https://storage-backend-prd-useast1.realtime.co/authenticate";
        private const string putItemURL = "https://storage-backend-prd-useast1.realtime.co/putItem";

    
        public brokerResult CreateChannel(Guid eventID, IUser eventCreator, IEnumerable<IUser> invitees)
        {
            string tableName = RTFHelpers.eidToEtableName(eventID);
            string data = RTFHelpers.MakeCreateString(tableName);

            try {
                string creationResult = RTFHelpers.SendRequest(CreateUrl, data);
            } catch (WebException e) { 
                return new brokerResult { type = ResultType.createError, reason = ErrorReason.remoteCreateFailure, errorMessage = e.Message };
            }

            string result = AuthenticateUser(eventCreator, tableName);
            if (result == tableName)
            {
                foreach (IUser invitee in invitees)
                { AuthenticateUser(invitee, tableName); }
            }
            else return new brokerResult { type = ResultType.createError, reason = ErrorReason.remoteAuthFailure, errorMessage = result };

            return new brokerResult { type = ResultType.success };
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
            string data = RTFHelpers.MakeAuthString(tableNames, userToken);
            try { string result = RTFHelpers.SendRequest(AuthUrl, data); } 
            catch (WebException e)
                { throw new Exception("Authorization failed. Code " + e.Message); }

            return "";
        }

        public brokerResult AddToChannel(IUser user, Guid eventId)
        {
            AuthenticateUser(user, RTFHelpers.eidToEtableName(eventId));
            return new brokerResult { type = ResultType.success };
        }

        //Current implementation only allows for all users to have the same (RU) policy on table. 
        //Otherwise we have to store each policy in DB.
        private string AuthenticateUser(IUser user, string tableName)
        {
            List<string> tableNames = user.Channels;
            tableNames.Add(tableName);
            string userToken = user.Token;
            string result = AuthUser(tableNames, userToken);
            user.Channels.Add(tableName);
            return result;
        }

        public brokerResult PutInvite(IUser user, string eventTable)
        {
            string userToken = user.Token;
            string userTableName = user.InvitesChannel;
            string data = RTFHelpers.MakeInviteString(userToken, userTableName, eventTable);
            string result = RTFHelpers.SendRequest(putItemURL, data);
            return new brokerResult { type = ResultType.success }; ;
        }

        public brokerResult RespondToInvite(IUser user, Guid eventID, bool accept)
        {



            return new brokerResult { type = ResultType.unimplemented };
        }

        

        

    }
}
