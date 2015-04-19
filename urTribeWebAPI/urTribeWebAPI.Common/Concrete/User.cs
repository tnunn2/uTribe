using System;
using System.Collections.Generic;
using urTribeWebAPI.Common.Interfaces;

namespace urTribeWebAPI.Common.Concrete
{
    public class User : IUser
    {

        #region Member Variables
        #endregion

        #region Properties
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string Token
        {
            get { return ID.ToString(); }
        }

        public string InvitesChannel { get; set; }

        public List<string> Channels { get; set; }
        #endregion

    }
}
