using System;
using System.Collections.Generic;
using System.Net;
using urTribeWebAPI.Common;
using urTribeWebAPI.Messaging;
using System.Configuration;

namespace urTribeWebAPI.Messaging
{
    //Refactored Version.  New methods are not part of the interfaces.
    public class RealTimeBroker_N : IMessageBroker
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
        public RealTimeBroker_N(IRTFStringBuilder rtfStringBuilder, IMessageConnect messageConnect)
        {
            _rtfStringBuilder = rtfStringBuilder;
            _messageConnect = messageConnect;
        }
        public RealTimeBroker_N ()
        {
            _messageConnect = new RealTimeFrameworkConnect(); 
            _rtfStringBuilder = new RTFStringBuilder();
        }
        #endregion


        #region Public Method Old
        public BrokerResult CreateAuthAndInvite(Guid eventID, Common.IUser eventCreator, IEnumerable<Common.IUser> invitees)
        {
            throw new NotImplementedException();
        }

        public BrokerResult AddToChannel(Common.IUser user, Guid eventId)
        {
            throw new NotImplementedException();
        }

        public BrokerResult RespondToInvite(Common.IUser user, Guid eventID, bool accept)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Public Method
        public BrokerResult AuthUser(List<string> tableNames, string userToken)
        {
            try
            {
                //bool busy = true;
                string data = RTStringBuilder.MakeAuthString(tableNames, userToken);

                var AuthUrl = ConfigurationManager.AppSettings["RealtimeBrokerAuthUrl"];
                string result = MessageConnect.SendRequest(AuthUrl, data);

                return BrokerResult.newSuccess();
            }
            catch (WebException e)
            {
                return new BrokerResult { type = ResultType.authError, errorMessage = e.Message };
            }

        }
        public BrokerResult CreateEventChannel(string tableName, IUser eventCreator)
        {
            string data = RTStringBuilder.MakeCreateString(tableName);

            try
            {
                var CreateUrl = ConfigurationManager.AppSettings["RealtimeBrokerCreateUrl"];
                string creationResult = MessageConnect.SendRequest(CreateUrl, data);
                return BrokerResult.newSuccess();
            }
            catch (WebException e)
            {
                return new BrokerResult { type = ResultType.createError, reason = ErrorReason.remoteCreateFailure, errorMessage = e.Message };
            }
        }
        public string CreateUserChannel(IUser user)
        {
            string tableName = "user" + user.ID;
            string data = RTStringBuilder.MakeCreateString(tableName);
            return tableName;
        }
        public List<string> ConvertEventToTableName(IEnumerable<IEvent> events)
        {
            var newEventList = new List<string>();
            foreach (var evt in events)
                newEventList.Add(evt.ID.ToString());
            return newEventList;
        }
        public BrokerResult SendInvite(IUser user, string eventTable, string invitedBy)
        {
            string userToken = user.Token;
            string userTableName = user.InvitesChannel;
            string data = RTStringBuilder.MakeInviteString(userToken, userTableName, eventTable, invitedBy);

            var putItemURL = ConfigurationManager.AppSettings["RealtimeBrokerPutItemURL"];
            string result = MessageConnect.SendRequest(putItemURL, data);

            return new BrokerResult { type = ResultType.fullsuccess }; ;
        }
        #endregion
    }
}
