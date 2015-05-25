using System;
using System.Collections.Generic;
using urTribeWebAPI.Common;

namespace urTribeWebAPI.Messaging
{
    public interface IMessageBroker
    {
        //BrokerResult CreateEventChannel(Guid eventID, IUser eventCreator);
        BrokerResult CreateAuthAndInvite(Guid eventID, IUser eventCreator, IEnumerable<IUser> invitees);
        BrokerResult AddToChannel(IUser user, Guid eventId);
        BrokerResult RespondToInvite(IUser user, Guid eventID, bool accept);
        BrokerResult CreateUserChannel(IUser user);
        BrokerResult InviteUsers(IEnumerable<IUser> invitees, string inviter, string tableName);
    }
}
