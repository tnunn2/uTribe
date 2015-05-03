using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urTribeWebAPI.Models
{
    public class sampleEventFactory
    {

        public static EventDescription getEvent1(Guid userID, Guid eventID)
        {
            Guid sampleID = new Guid("a90a99cc-8706-40be-a9a1-332db93e66b4");
            if (userID == sampleID)
            {
                return new EventDescription()
                {
                    success = true,
                    eventID = new Guid("7c74ac99-4cdf-4f3a-a63b-fc040f300607"),
                    eventName = "Thirty Years Of Catherine",
                    hostName = "Catherine C",
                    hostID = new Guid("aa918dde-94e0-4323-a281-c8274d67eaca"),
                    startTime = new DateTime(2015, 5, 3, 18, 45, 0),
                    endTime = new DateTime(2015, 5, 3, 23, 59, 0),
                    locationName = "Chateau Lakeside",
                    invitedByName = "Benjamin D",
                    invitedByGuid = new Guid("b40354dc-5734-432e-b6c5-24adf8890312"),
                    LocationGps = new GPS()
                    {
                        latitude = .9685768,
                        longitude = -87.6502136
                    }
                };
            }
            else return new EventDescription() {success = false};
        }

        public static EventListResponse user1Events(Guid userID)
        {
            Guid sampleID = new Guid("a90a99cc-8706-40be-a9a1-332db93e66b4");
            if (userID == sampleID)
            {
                EventDescription e1 = getEvent1(userID, new Guid());
                return new EventListResponse()
                {
                    success = true,
                    goingEvents = new List<EventDescription>()
                    {
                        e1
                    },
                    invitedEvents = new List<EventDescription>() {},
                    maybeEvents = new List<EventDescription>() {}
                };
            }
            return new EventListResponse() {success = false};
        }
    }
}