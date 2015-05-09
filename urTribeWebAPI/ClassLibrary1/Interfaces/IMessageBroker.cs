using System;
using System.Collections.Generic;
using urTribeWebAPI.Common.Interfaces;

namespace urTribeWebAPI.Messaging
{
    public interface IMessageBroker
    {

        //brokerResult CreateEventChannel(Guid eventID, IUser eventCreator);
        brokerResult CreateAuthAndInvite(Guid eventID, IUser eventCreator, IEnumerable<IUser> invitees);
        string CreateUserChannel(IUser user);
        brokerResult AddToChannel(IUser user, Guid eventId);

        brokerResult RespondToInvite(IUser user, Guid eventID, bool accept);
    }
}
