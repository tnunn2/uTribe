using System;
using urTribeWebAPI.Common.Interfaces;

namespace urTribeWebAPI.Common.Concrete
{
    public class ScheduledEvent : IEvent
    {
        public Guid ID
        {
            get;
            set;
        }
    }
}
