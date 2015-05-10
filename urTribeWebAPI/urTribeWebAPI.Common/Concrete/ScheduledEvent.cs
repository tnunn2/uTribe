using System;
using System.Collections.Generic;


namespace urTribeWebAPI.Common
{
    public class ScheduledEvent : IEvent
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; } 
        public DateTime Time { get; set; }
        public string Location { get; set; }
        public Address LocationAddress { get; set; }

//        public List<IUser> invitedUsers { get; set; }

//        public List<IUser> attendingUsers { get; set; }
    }
}
