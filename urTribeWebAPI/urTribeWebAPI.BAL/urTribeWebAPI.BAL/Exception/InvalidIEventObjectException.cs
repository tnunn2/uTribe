using System;

namespace urTribeWebAPI.BAL
{
    public class InvalidIEventObjectException : Exception
    {
        #region Constant
        private const string MESSAGE = "Invalid IEvent object.";
        #endregion
        public InvalidIEventObjectException() : base(MESSAGE)
        {
        }

        public InvalidIEventObjectException(string message)
            : base(message)
        {
        }


        public InvalidIEventObjectException(Exception inner)
            : base(MESSAGE, inner)
        {
        }

    }

}
