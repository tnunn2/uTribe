using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urTribeWebAPI.Models
{
    public class EventDescription
    {
        public bool success { get; set; }
        public Guid eventID { get; set; }
        public string eventName { get; set; }
        public string hostName { get; set; }
        public Guid hostID { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string locationName { get; set; }
        public string invitedByName { get; set; }
        public Guid invitedByGuid { get; set; }
        public GPS LocationGps { get; set; }
    }
}