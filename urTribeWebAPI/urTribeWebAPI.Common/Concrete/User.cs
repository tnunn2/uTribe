using System;
using System.Collections.Generic;


namespace urTribeWebAPI.Common
{
    public class User : IUser
    {

        #region Member Variables
        #endregion

        #region Properties
        public Guid ID 
        { 
            get; 
            set; 
        }

        public string Name 
        { 
            get; 
            set; 
        }

        public UserCurrentStatus Status 
        { 
            get; 
            set; 
        }

        //Used for Authorization in the Real Time Framework
        public string Token
        {
            get { return ID.ToString(); }
        }

        //Used to put out invitation to an event???
        public string InvitesChannel 
        { 
            get; 
            set; 
        }

        //List of Events???  Is this necessary???
        public List<string> Channels 
        { 
            get; 
            set; 
        }
        #endregion

    }
}
