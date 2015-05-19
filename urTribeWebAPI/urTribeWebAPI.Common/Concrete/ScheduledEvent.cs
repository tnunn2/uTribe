using System;
using System.Collections.Generic;


namespace urTribeWebAPI.Common
{
    public class ScheduledEvent : IEvent
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; } 
        public long Time { get; set; }
        public string Location { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

//        public List<IUser> invitedUsers { get; set; }

//        public List<IUser> attendingUsers { get; set; }
    }
}
