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

        void CreateEventChannel(Guid eventID, IUser eventCreator, IEnumerable<IUser> invitees);
        void CreateAuthAndInvite(string eventID, IUser eventCreator, IEnumerable<IUser> invitees);
        string CreateUserChannel(IUser user);
        void AddToChannel(IUser user, Guid eventId);
    }
}
