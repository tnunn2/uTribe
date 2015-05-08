using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using urTribeWebAPI.Common;
using urTribeWebAPI.Common.Interfaces;
using urTribeWebAPI.Common.Concrete;
using urTribeWebAPI.BAL;

namespace urTribeWebAPI.Controllers
{
    public class EventsController : ApiController
    {
        //Create a newEvent
        //Update Event Info
        public Guid Post (Guid userId, ScheduledEvent evt)
        {

            if (evt.ID == new Guid())
            {
                using (UserFacade userFacade = new UserFacade())
                {
                    IUser user = userFacade.FindUser(userId);
                    if (user == null)
                        return new Guid("99999999-9999-9999-9999-999999999999");

                    var eventId = userFacade.CreateEvent(user, evt);
                    return eventId;
                }
            }
            else
            {
                using (EventFacade eventFacade = new EventFacade())
                {
                    if (userId == eventFacade.EventOwner(evt))
                    {
                        eventFacade.UpdateEvent(evt);
                        return evt.ID;
                    }
                    else
                        return new Guid("99999999-9999-9999-9999-999999999999");
                }
            }
        }

        //Add new contacts to events
        //Maybe change to take a list of Contacts???
        public void Post (Guid userId, Guid eventId)
        {
            using (UserFacade userFacade = new UserFacade())
            using (EventFacade eventFacade = new EventFacade())
            {
                IUser user = userFacade.FindUser(userId);
                IEvent evt = eventFacade.FindEvent(eventId);
                eventFacade.AddContactToEvent(user, evt);
            }
        }

        //Update Contacts attendents status
        public void Post (Guid userId, Guid eventId, EventAttendantsStatus attendStatus)
        {
            using (UserFacade userFacade = new UserFacade())
            using (EventFacade eventFacade = new EventFacade())
            {
                IUser user = userFacade.FindUser(userId);
                IEvent evt = eventFacade.FindEvent(eventId);
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
