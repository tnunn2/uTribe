using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urTribeWebAPI.Models
{
    public class EventListResponse
    {
        public bool success { get; set; }
        public List<EventDescription> invitedEvents { get; set; }
        public List<EventDescription> maybeEvents { get; set; }
        public List<EventDescription> goingEvents { get; set; }
    }
}