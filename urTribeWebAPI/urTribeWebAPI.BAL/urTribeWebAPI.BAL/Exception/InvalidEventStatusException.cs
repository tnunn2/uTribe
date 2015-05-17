using System;
using urTribeWebAPI.Common;

namespace urTribeWebAPI.BAL
{
    public class InvalidEventStatusException : Exception
    {
        #region Constant
        private const string MESSAGE = "The specified status can not be used on this event in this situation";
        #endregion
        public InvalidEventStatusException()
            : base(MESSAGE)
        {
        }

        public InvalidEventStatusException(EventAttendantsStatus attendStatus)
            : base(string.Format(MESSAGE+": {0} ", attendStatus.ToString()))
        {
        }
        public InvalidEventStatusException(string message)
            : base(message)
        {
        }


        public InvalidEventStatusException(Exception inner)
            : base(MESSAGE, inner)
        {
        }

    }
}
