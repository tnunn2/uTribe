using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI.WebControls;
using Moq;
using urTribeWebAPI.Models;

namespace urTribeWebAPI.Controllers
{
    public class EventController : ApiController
    {
        private readonly IMessageBroker _broker = new RealtimeBroker();
        private readonly Repository _repo = new Repository();


        [HttpPut]
        [ActionName("CreateEvent")]
        public string PutEvent(Guid creatorGuid)
        {
            User creator = _repo.findUserByID(creatorGuid);
            List < User > invitees = new List<User>();

            //TODO: get invitees from request body or subsequent put requests

            int eventID = _repo.newEvent(creator, invitees);
            _broker.CreateChannel(eventID, creator, invitees);

            //TODO: return eventID to client in HTTP response
        }

        [HttpPut]
        [ActionName("InviteTo")]
        public void AddInvitee(Guid inviteeGuid, int eventID)
        {
            User invitee = _repo.findUserByID(inviteeGuid);
            _broker.AddToChannel(invitee, eventID);
        }

    }
}
