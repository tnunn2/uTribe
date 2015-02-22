using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Web;

namespace urTribeWebAPI.Models
{
    public class Event
    {
        //None of this is right I know
        public int Id { get; set; }
        public List<User> Participants { get; set; }
        private int Channel { get; set; }

    }

    public class User
    {
        public User(string name)
        {
            throw new NotImplementedException();
        }
    }
}
}