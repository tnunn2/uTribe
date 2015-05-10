using System;
using System.Collections.Generic;


namespace urTribeWebAPI.Common
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
