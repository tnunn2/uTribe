using System;
using System.Collections.Generic;


namespace urTribeWebAPI.Common
{
    public class User : IUser
    {

        #region Member Variables
        private string invitesChannel;
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
            get {
            if (invitesChannel == null) return "user"+ID;
            else return invitesChannel;
            }
            set { invitesChannel = value; } 
        }

        //We have to keep a list of the channels they're authenticated on, because RTF.
        //Deprecated. From now on will get events instead.
        public List<string> AuthenticatedChannels 
        { 
            get; 
            set; 
        }
        #endregion

    }
}
