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

        //Invites and Contact Requests go here
        public string UserChannel 
        { 
            get; 
            set; 
        }

        //We have to keep a list of the channels they're authenticated on, because RTF.
        public List<string> AuthenticatedChannels 
        { 
            get; 
            set; 
        }
        #endregion

    }
}
