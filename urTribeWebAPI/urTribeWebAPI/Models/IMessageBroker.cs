using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using urTribeWebAPI.Common.Interfaces;

namespace urTribeWebAPI.Models
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
