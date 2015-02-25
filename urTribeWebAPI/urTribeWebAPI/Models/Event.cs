using System;
using System.Collections.Generic;

namespace urTribeWebAPI.Models
{
    public class Event
    {
        //None of this is right I know
        public int Id { get; set; }
        public List<User> Participants { get; set; }
        public int Channel { get; set; }

    }

    
}