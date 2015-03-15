using System;
using System.Collections.Generic;
using urTribeWebAPI.Common.Interfaces;

namespace urTribeWebAPI.Models
{
    public class Event
    {
        //None of this is right I know
        public int Id { get; set; }
        public List<IUser> Participants { get; set; }
        public int Channel { get; set; }

    }

    
}
