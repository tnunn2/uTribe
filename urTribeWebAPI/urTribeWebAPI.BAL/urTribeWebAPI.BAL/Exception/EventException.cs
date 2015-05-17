using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urTribeWebAPI.BAL
{
    public class EventException : Exception
    {
        #region Constant
        private const string MESSAGE = "An exception occured involving an Event object";
        #endregion
        public EventException()
            : base(MESSAGE)
        {
        }

        public EventException(Guid eventId)
            : base(string.Format(MESSAGE+": {0} ", eventId.ToString()))
        {
        }
        public EventException(string message)
            : base(message)
        {
        }


        public EventException(Exception inner)
            : base(MESSAGE, inner)
        {
        }
    }
}
