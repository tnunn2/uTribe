using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urTribeWebAPI.Models
{
    public class Repository
    {
        public User findUserByID(Guid id)
        {
            throw new NotImplementedException();
        }

        //returns event ID
        public int newEvent(User creator, List<User> invitees)
        {
            throw new NotImplementedException();
        }

    }
}