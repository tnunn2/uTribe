using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace urTribeWebAPI.Models
{
    interface IMessageBroker
    {

        void CreateChannel(Guid eventID, User eventCreator, List<User> invitees);
        void AddToChannel(User inviteeGuid, int eventId);
    }
}
