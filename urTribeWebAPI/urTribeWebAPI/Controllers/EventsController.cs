using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using urTribeWebAPI.Common;
using urTribeWebAPI.BAL;
using urTribeWebAPI.Messaging;

namespace urTribeWebAPI.Controllers
{
    public class EventsController : ApiController
    {
        //Create a newEvent
        //Update Event Info

        //TODO: Need to figure out what to return... 
        public Guid Post (Guid userId, ScheduledEvent evt)
        {
            Guid eventId = new Guid();
            BrokerResult brokerResults; 

            if (evt.ID == new Guid())
            {
                using (UserFacade userFacade = new UserFacade())
                    brokerResults = userFacade.CreateEvent(userId, evt);
            }
            else
            {
                using (EventFacade eventFacade = new EventFacade())
                    eventId = eventFacade.UpdateEvent(userId, evt);
            }

            return eventId;
            // return brokerResults;
        }

        //Add new contacts to events
        public void Post(Guid userId, Guid eventId, List<Guid> contactList)
        {
            using (EventFacade eventFacade = new EventFacade())
                eventFacade.AddContactsToEvent(userId, eventId, contactList);
        }

        //Update Contacts attendents status
        public void Post (Guid userId, Guid eventId, EventAttendantsStatus attendStatus)
        {
            if (attendStatus == EventAttendantsStatus.Cancel)
            {
                using (UserFacade userFacade = new UserFacade())
                    userFacade.CancelEvent(userId, eventId);
            }
            else
            {
                using (EventFacade eventFacade = new EventFacade())
                    eventFacade.ChangeContactAttendanceStatus(userId, eventId, attendStatus);
            }
        }

        //Return all Active events
        public IEnumerable<IEvent> Get (Guid userId)
        {
            using (UserFacade userFacade = new UserFacade())
            {
                IEnumerable<IEvent> eventList = userFacade.RetrieveEventsByAttendanceStatus(userId, EventAttendantsStatus.All);
                return eventList;
            }
        }

        //Find an event ???
        //InviteResponse ???
    }
}
