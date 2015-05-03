using System;
using System.Collections.Generic;
using System.Net;
using urTribeWebAPI.Common.Interfaces;
using urTribeWebAPI.Messaging;

// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace urTribeWebAPI.Messaging
{
    public class RealtimeBroker : IMessageBroker
    {
        //Things we can't change

        private const string CreateUrl = "https://storage-backend-prd-useast1.realtime.co/createTable";
        private const string AuthUrl = "https://storage-backend-prd-useast1.realtime.co/authenticate";
        private const string putItemURL = "https://storage-backend-prd-useast1.realtime.co/putItem";


        public brokerResult CreateAuthAndInvite(Guid eventID, IUser eventCreator, IEnumerable<IUser> invitees)
        {
            string tableName = RTFHelpers.eidToEtableName(eventID);
            brokerResult creationRes = CreateEventChannel(tableName, eventCreator);
            if (creationRes.ok())
            {
                return inviteUsers(invitees, tableName);
            }
            else return creationRes;
        }

        //Does not evaluate JSON response for errors in table creation!
        //doesn't invite!
        public brokerResult CreateEventChannel(string tableName, IUser eventCreator)
        {
            string data = RTFHelpers.MakeCreateString(tableName);

            try {
                string creationResult = RTFHelpers.SendRequest(CreateUrl, data);
            } catch (WebException e) { 
                return new brokerResult { type = ResultType.createError, reason = ErrorReason.remoteCreateFailure, errorMessage = e.Message };
            }

            bool creatorOK = AuthenticateAndUpdateUser(eventCreator, tableName);
            if (creatorOK)
            {
                return brokerResult.newSuccess();
            }
            else return new brokerResult { type = ResultType.createError, reason = ErrorReason.remoteAuthFailure};
        }

        public brokerResult inviteUsers(IEnumerable<IUser> invitees, string tableName) {
            brokerResult invitesResult = brokerResult.newInviteResult();
            foreach (IUser invitee in invitees)
            { 
                bool success = AuthenticateAndUpdateUser(invitee, tableName);
                if (success) invitesResult.validUsers.Add(invitee);
                else invitesResult.invalidUsers.Add(invitee);
            }
            if (invitesResult.invalidUsers.Count > 0)
            {
                invitesResult.type = ResultType.sufficientSuccess;
                if (invitesResult.validUsers.Count == 0)
                {
                    invitesResult.type = ResultType.inviteError;
                    invitesResult.reason = ErrorReason.remoteAuthFailure;
                }
            }
            return invitesResult;
        }

        /*TODO: 
         * 1)DONE
         * 2)write unit tests --sorta done
         * 3)exception handling
         * 4)DONE
         * 5)implement wait for resource busy "creating" state
        */
        private brokerResult AuthUser(List<string> tableNames, string userToken)
        {
            //bool busy = true;
            string data = RTFHelpers.MakeAuthString(tableNames, userToken);
            try { string result = RTFHelpers.SendRequest(AuthUrl, data); }
            catch (WebException e)
            { return new brokerResult { type = ResultType.authError, errorMessage = e.Message }; }

            return brokerResult.newSuccess();
        }

        public brokerResult AddToChannel(IUser user, Guid eventId)
        {
            AuthenticateAndUpdateUser(user, RTFHelpers.eidToEtableName(eventId));
            return brokerResult.newSuccess();
        }

        //Current implementation only allows for all users to have the same (RU) policy on table. 
        //Otherwise we have to store each policy in DB.
        public bool AuthenticateAndUpdateUser(IUser user, string tableName)
        {
            List<string> tableNames = user.Channels;
            tableNames.Add(tableName);
            string userToken = user.Token;
            brokerResult result = AuthUser(tableNames, userToken);
            if (result.ok()) user.Channels.Add(tableName);
            return result.ok();
        }

        public brokerResult PutInvite(IUser user, string eventTable, string invitedBy)
        {
            string userToken = user.Token;
            string userTableName = user.InvitesChannel;
            string data = RTFHelpers.MakeInviteString(userToken, userTableName, eventTable, invitedBy);
            string result = RTFHelpers.SendRequest(putItemURL, data);
            return new brokerResult { type = ResultType.fullsuccess }; ;
        }

        public brokerResult RespondToInvite(IUser user, Guid eventID, bool accept)
        {
            bool success = true;
            if (accept) {
                string tablename = RTFHelpers.eidToEtableName(eventID);
                success = AuthenticateAndUpdateUser(user, tablename);
            }
            if (success) return brokerResult.newSuccess();
            else return new brokerResult { type = ResultType.respondError, reason = ErrorReason.remoteAuthFailure };
        }
        public string CreateUserChannel(IUser user)
        {
            string tableName = RTFHelpers.userToTableName(user);
            string data = RTFHelpers.MakeCreateString(tableName);
            return tableName;
        }
        

        

    }
}
