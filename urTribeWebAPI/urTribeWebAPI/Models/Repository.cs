using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urTribeWebAPI.Models
{
    public class Repository
    {
        private Dictionary<Guid, User> users;
        private Dictionary<Guid, Event> events;

        public Repository()
        {
            users = new Dictionary<Guid, User>();
            events = new Dictionary<Guid, Event>();
        }

        public User findUserByID(Guid id)
        {
            User result = null;
            if (users.TryGetValue(id, out result)) return result;
            else throw new ArgumentException();
        }

        //returns event ID
        public Guid newEvent(User creator, List<User> invitees)
        {
            throw new NotImplementedException();
        }

    }
}