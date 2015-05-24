using System;
using System.Collections.Generic;


namespace urTribeWebAPI.Common
{
    public class ScheduledEvent : IEvent
    {
        #region Member Variable
        private string _time;
        #endregion

        public Guid ID { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; } 
        public string Time 
        {
            get
            {
                return _time;
            }

            set
            {
                DateTime dt;
                if (DateTime.TryParse(value, null, System.Globalization.DateTimeStyles.RoundtripKind, out dt))
                    _time = value;
                else
                    throw new Exception("Error Setting the Event.Time property.  Value incorrect format.");
            }
        }
        public string Location { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Description { get; set; }

        //        public List<IUser> invitedUsers { get; set; }

        //        public List<IUser> attendingUsers { get; set; }

        #region Public Method
        public DateTime TimeAsDateTime ()
        {
            DateTime dt = DateTime.Parse(Time,  null, System.Globalization.DateTimeStyles.RoundtripKind);
            return dt;
        }
        #endregion
    }
}
