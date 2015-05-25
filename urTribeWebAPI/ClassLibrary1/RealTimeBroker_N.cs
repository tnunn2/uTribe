using System;
using System.Collections.Generic;
using System.Net;
using urTribeWebAPI.Common;
using urTribeWebAPI.Messaging;
using System.Configuration;
using System.Threading;

namespace urTribeWebAPI.Messaging
{
    //Refactored Version.  New methods are not part of the interfaces.
    public class RealTimeBroker_N //: IMessageBroker
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

                var AuthUrl = Properties.Settings.Default.RTFAuthURL;
                string result = MessageConnect.SendRequest(AuthUrl, data);

                return BrokerResult.newSuccess();
            }
            catch (WebException e)
            {
                return new BrokerResult { type = ResultType.authError, Message = e.Message };
            }

        }
        public BrokerResult CreateEventChannel(string tableName, IUser eventCreator)
        {
            string data = RTStringBuilder.MakeCreateString(tableName);

            try
            {
                var CreateUrl = Properties.Settings.Default.RTFCreateURL;
                string creationResult = MessageConnect.SendRequest(CreateUrl, data);
                return BrokerResult.newSuccess();
            }
            catch (WebException e)
            {
                return new BrokerResult { type = ResultType.createError, reason = ErrorReason.remoteCreateFailure, Message = e.Message };
            }
        }
        public BrokerResult CreateUserChannel(IUser user)
        {
            string tableName = UserToTableName(user);
            string data = RTStringBuilder.MakeCreateString(tableName);
            string createUrl = Properties.Settings.Default.RTFCreateURL;
            MessageConnect.SendRequest(createUrl, data);

            Thread.Sleep(Properties.Settings.Default.RTFCreationSleepTime);

            AuthUser(new List<string>() { tableName }, user.Token);

            BrokerResult result = BrokerResult.newSuccess();
            result.Message = tableName;
            return result;
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
            string userTableName = user.UserChannel;
            string data = RTStringBuilder.MakeInviteString(userToken, userTableName, eventTable, invitedBy);

            var putItemURL = Properties.Settings.Default.RTFPutURL;
            string result = MessageConnect.SendRequest(putItemURL, data);

            return new BrokerResult { type = ResultType.fullsuccess }; ;
        }
        #endregion

        private string UserToTableName(IUser user)
        {
            return "user" + user.ID;
        }
    }
}
