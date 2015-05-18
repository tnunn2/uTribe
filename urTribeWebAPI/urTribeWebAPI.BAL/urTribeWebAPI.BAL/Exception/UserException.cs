using System;

namespace urTribeWebAPI.BAL
{
    public class UserException : Exception
    {
        #region Constant
        private const string MESSAGE = "An exception occured involving an User object";
        #endregion
        public UserException()
            : base(MESSAGE)
        {
        }

        public UserException(Guid userId)
            : base(string.Format(MESSAGE+": {0} ", userId.ToString()))
        {
        }
        public UserException(string message)
            : base(message)
        {
        }


        public UserException(Exception inner)
            : base(MESSAGE, inner)
        {
        }
    }
}