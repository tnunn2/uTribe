using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using urTribeWebAPI.Common.Interfaces;

namespace urTribeWebAPI.Models
{
    interface IMessageBroker
    {

        brokerResult CreateChannel(Guid eventID, IUser eventCreator, IEnumerable<IUser> invitees);
        brokerResult AddToChannel(IUser user, Guid eventId);

        brokerResult RespondToInvite(IUser user, Guid eventID, bool accept);
    }
}
