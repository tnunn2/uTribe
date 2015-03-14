using System;
using System.Collections.Generic;
using urTribeWebAPI.Common.Interfaces;

namespace urTribeWebAPI.Common.Concrete
{
    public class User : IUser
    {
        public Guid ID
        {
            get;
            set;
        }

        public string Token
        {
            get { return ID.ToString(); }
        }

        public string InvitesChannel { get; set; }


        public List<string> Channels { get; set; }


    }
}
