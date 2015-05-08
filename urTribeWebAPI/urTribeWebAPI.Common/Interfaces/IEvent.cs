using System;
using System.Collections.Generic;

namespace urTribeWebAPI.Common.Interfaces
{
    public interface IEvent : IDBRepositoryObject
    {
        bool Active { get; set; } 

        string VenueName { get; set; }

        List<IUser> invitedUsers { get; set; }

        List<IUser> attendingUsers{ get; set; }
    }
}
