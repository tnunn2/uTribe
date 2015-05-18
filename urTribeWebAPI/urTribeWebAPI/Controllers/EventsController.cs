using System;
using System.Collections.Generic;
using System.Web.Http;
using urTribeWebAPI.BAL;
using urTribeWebAPI.Common;
using urTribeWebAPI.Messaging;
using urTribeWebAPI.Models.Response;

namespace urTribeWebAPI.Controllers
{
    public class EventsController : ApiController
    {
        //Create a newEvent
        //Update Event Info
        public APIResponse Post (Guid userId, ScheduledEvent evt)
        {
            try
            {
                if (evt.ID == new Guid())
                {
                    using (UserFacade userFacade = new UserFacade())
                    {
                        Guid result = userFacade.CreateEvent(userId, evt);

                        APIResponse response = new APIResponse(APIResponse.ReponseStatus.success, new { EventId = result});
                        return response;
                    }
                }
                else
                {
                    using (EventFacade eventFacade = new EventFacade())
                        eventFacade.UpdateEvent(userId, evt);

                    APIResponse response = new APIResponse(APIResponse.ReponseStatus.success, null);
                    return response;
                }
            }
            catch (Exception ex)
            {
                APIResponse response = new APIResponse(APIResponse.ReponseStatus.error, new {Error = ex.Message});
                return response;
            }
        }

        //Add new contacts to events
        public APIResponse Post(Guid userId, Guid eventId, List<Guid> contactList)
        {
            try
            {
                using (EventFacade eventFacade = new EventFacade())
                    eventFacade.AddContactsToEvent(userId, eventId, contactList);

                APIResponse response = new APIResponse(APIResponse.ReponseStatus.success, null);
                return response;
            }
            catch (Exception ex)
            {
                APIResponse response = new APIResponse(APIResponse.ReponseStatus.error, new { Error = ex.Message });
                return response;
            }

        }

        //Update Contacts attendents status
        public APIResponse Post(Guid userId, Guid eventId, EventAttendantsStatus attendStatus)
        {
            try
            {
                if (attendStatus == EventAttendantsStatus.Cancel)
                {
                    using (UserFacade userFacade = new UserFacade())
                        userFacade.CancelEvent(userId, eventId);

                    APIResponse response = new APIResponse(APIResponse.ReponseStatus.success, null);
                    return response;
                }
                else
                {
                    using (EventFacade eventFacade = new EventFacade())
                        eventFacade.ChangeContactAttendanceStatus(userId, eventId, attendStatus);

                    APIResponse response = new APIResponse(APIResponse.ReponseStatus.success, null);
                    return response;

                }
            }
            catch (Exception ex)
            {
                APIResponse response = new APIResponse(APIResponse.ReponseStatus.error, new { Error = ex.Message });
                return response;
            }
        }

        //Return all Active events
        public APIResponse Get (Guid userId)
        {
            try
            {
                using (UserFacade userFacade = new UserFacade())
                {
                    IEnumerable<IEvent> eventList = userFacade.RetrieveEventsByAttendanceStatus(userId, EventAttendantsStatus.All);

                    if (eventList == null)
                    {
                        string message = "Unable to return a list of events.";
                        APIResponse response = new APIResponse(APIResponse.ReponseStatus.fail, new { Error = message });
                        return response;
                    }
                    else
                    {
                        APIResponse response = new APIResponse(APIResponse.ReponseStatus.success, new { EventList = eventList });
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                APIResponse response = new APIResponse(APIResponse.ReponseStatus.error, new { Error = ex.Message });
                return response;
            }
        }

        //Find an event ???
        //InviteResponse ???
    }
}
