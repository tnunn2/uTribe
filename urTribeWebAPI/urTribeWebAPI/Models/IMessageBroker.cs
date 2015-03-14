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

        void CreateChannel(Guid eventID, IUser eventCreator, IEnumerable<IUser> invitees);
        void AddToChannel(IUser user, Guid eventId);
    }
}
