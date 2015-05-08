using System;
using System.Collections.Generic;
using urTribeWebAPI.Common.Interfaces;

namespace urTribeWebAPI.Common.Concrete
{
    public class ScheduledEvent : IEvent
    {
        public Guid ID { get; set; }

        public bool Active { get; set; } 
        
        public string VenueName { get; set; }

        public List<IUser> invitedUsers { get; set; }

        public List<IUser> attendingUsers { get; set; }

    }
}
