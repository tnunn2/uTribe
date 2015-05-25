using System;
using System.Collections.Generic;
using System.Net;
using urTribeWebAPI.Common;
using urTribeWebAPI.Messaging;
using System.Configuration;
using System.Threading;

namespace urTribeWebAPI.Messaging
{
    public class RealTimeBroker : IMessageBroker
    {
        #region ReadOnly
        private readonly IRTFStringBuilder _rtfStringBuilder;
        private readonly IMessageConnect _messageConnect;
        #endregion

        #region Properties
        private IMessageConnect MessageConnect
        {
            get
            {
                return _messageConnect;
            }
        }
        private IRTFStringBuilder RTStringBuilder
        {
            get
            {
                return _rtfStringBuilder;
            }
        }
        #endregion

        #region Constructor
        //Testing Seam
        public RealTimeBroker(IRTFStringBuilder rtfStringBuilder, IMessageConnect messageConnect)
        {
            _rtfStringBuilder = rtfStringBuilder;
            _messageConnect = messageConnect;
        }
        public RealTimeBroker ()
        {
            _messageConnect = new RealTimeFrameworkConnect(); 
            _rtfStringBuilder = new RTFStringBuilder();
        }
        #endregion

        #region Public Methods
        public BrokerResult CreateAuthAndInvite(Guid eventID, IUser eventCreator, IEnumerable<IUser> invitees)
        {
            string tableName = EIDToEtableName(eventID);
            BrokerResult creationRes = CreateEventChannel(eventID, eventCreator);
            if (creationRes.ok())
            {
                return InviteUsers(invitees, eventCreator.Name, tableName);
            }
            else return creationRes;
        }

        public BrokerResult AddToChannel(IUser user, Guid eventId)
        {
            AuthenticateAndUpdateUser(user, EIDToEtableName(eventId));
            return BrokerResult.newSuccess();
        }

        //Current implementation only allows for all users to have the same (RU) policy on table. 
        //Otherwise we have to store each policy in DB.
        public BrokerResult RespondToInvite(IUser user, Guid eventID, bool accept)
        {
            bool success = true;
            if (accept) {
                string tablename = EIDToEtableName(eventID);
                success = AuthenticateAndUpdateUser(user, tablename);
            }
            if (success) return BrokerResult.newSuccess();
            else return new BrokerResult { type = ResultType.respondError, reason = ErrorReason.remoteAuthFailure };
        }

        //Creates a User channel and authenticates. Facade will update user object
        public BrokerResult CreateUserChannel(IUser user)
        {
            string tableName = UserToTableName(user);
            string data = RTStringBuilder.MakeCreateString(tableName);
            string createUrl = Properties.Settings.Default.RTFCreateURL;
            MessageConnect.SendRequest(createUrl, data);

            Thread.Sleep(Properties.Settings.Default.RTFCreationSleepTime);

            AuthUser(new List<string>() {tableName}, user.Token);

            BrokerResult result = BrokerResult.newSuccess();
            result.Message = tableName;
            return result;
        }

        //Does not evaluate JSON response for errors in table creation!
        //doesn't invite!
        public BrokerResult InviteUsers(IEnumerable<IUser> invitees, string inviter, string tableName)
        {
            BrokerResult invitesResult = BrokerResult.newInviteResult();
            foreach (IUser invitee in invitees)
            {
                bool success = AuthenticateAndUpdateUser(invitee, tableName);
                if (success)
                {
                    PutInvite(invitee, tableName, inviter);
                    invitesResult.validUsers.Add(invitee);
                }
                else
                    invitesResult.invalidUsers.Add(invitee);
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

        public BrokerResult CreateEventChannel(Guid eventID, IUser eventCreator)
        {
            string tableName = EIDToEtableName(eventID);
            string data = RTStringBuilder.MakeCreateString(tableName);

            try
            {
                var CreateUrl = Properties.Settings.Default.RTFCreateURL;

                string creationResult = MessageConnect.SendRequest(CreateUrl, data);
            }
            catch (WebException e)
            {
                return new BrokerResult { type = ResultType.createError, reason = ErrorReason.remoteCreateFailure, Message = e.Message };
            }

            bool creatorOK = AuthenticateAndUpdateUser(eventCreator, tableName);
            if (creatorOK)
            {
                return BrokerResult.newSuccess();
            }
            else return new BrokerResult { type = ResultType.createError, reason = ErrorReason.remoteAuthFailure };
        }


        public string JustCreateChannel(string tableName)
        {
            string data = RTStringBuilder.MakeCreateString(tableName);
            var CreateUrl = Properties.Settings.Default.RTFCreateURL;

            return MessageConnect.SendRequest(CreateUrl, data);
        }
        #endregion

        #region Private Methods
        

        /*TODO: 
         * 1)DONE
         * 2)write unit tests --sorta done
         * 3)exception handling
         * 4)DONE
         * 5)implement wait for resource busy "creating" state
        */
        private BrokerResult AuthUser(List<string> tableNames, string userToken)
        {
            try 
            {
                //bool busy = true;
                string data = RTStringBuilder.MakeAuthString(tableNames, userToken);

                var AuthUrl = Properties.Settings.Default.RTFAuthURL;
                string result = MessageConnect.SendRequest(AuthUrl, data); 

                return BrokerResult.newSuccess();
            }
            catch (WebException e)
            { 
                return new BrokerResult { type = ResultType.authError, Message = e.Message }; 
            }

        }
       
        private BrokerResult PutInvite(IUser user, string eventTable, string invitedBy)
        {
            string userToken = user.Token;
            string userTableName = user.UserChannel;
            string data = RTStringBuilder.MakeInviteString(userToken, userTableName, eventTable, invitedBy);

            var putItemURL = Properties.Settings.Default.RTFPutURL;
            string result = MessageConnect.SendRequest(putItemURL, data);

            return new BrokerResult { type = ResultType.fullsuccess }; ;
        }
        private bool AuthenticateAndUpdateUser(IUser user, string tableName)
        {
            List<string> tableNames = user.AuthenticatedChannels;
            tableNames.Add(tableName);
            string userToken = user.Token;
            BrokerResult result = AuthUser(tableNames, userToken);
            if (result.ok()) user.AuthenticatedChannels.Add(tableName);
            return result.ok();
        }

        private string EIDToEtableName(Guid eventID)
        {
            string tableName = "event" + eventID;
            return tableName;
        }
        private string UserToTableName(IUser user)
        {
            return "user" + user.ID;
        }
        #endregion
    }
}
