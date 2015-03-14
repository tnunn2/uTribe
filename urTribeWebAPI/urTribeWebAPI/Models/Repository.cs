using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using urTribeWebAPI.Common.Interfaces;

namespace urTribeWebAPI.Models
{
    public class Repository
    {
        private Dictionary<Guid, IUser> users;
        private Dictionary<Guid, Event> events;

        public Repository()
        {
            users = new Dictionary<Guid, IUser>();
            events = new Dictionary<Guid, Event>();
        }

        public IUser findUserByID(Guid id)
        {
            IUser result = null;
            if (users.TryGetValue(id, out result)) return result;
            else throw new ArgumentException();
        }

        //returns event ID
        public Guid newEvent(IUser creator, List<IUser> invitees)
        {
            throw new NotImplementedException();
        }

    }
}