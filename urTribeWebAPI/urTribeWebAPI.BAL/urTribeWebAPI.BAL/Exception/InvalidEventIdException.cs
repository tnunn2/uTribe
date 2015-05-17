using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urTribeWebAPI.BAL
{
    public class InvalidEventIdException : Exception
    {
        #region Constant
        private const string MESSAGE = "Invalid eventId";
        #endregion
        public InvalidEventIdException()
            : base(MESSAGE)
        {
        }

        public InvalidEventIdException(Guid eventId)
            : base(string.Format(MESSAGE+": {0} ", eventId.ToString()))
        {
        }
        public InvalidEventIdException(string message)
            : base(message)
        {
        }


        public InvalidEventIdException(Exception inner)
            : base(MESSAGE, inner)
        {
        }

    }
}
