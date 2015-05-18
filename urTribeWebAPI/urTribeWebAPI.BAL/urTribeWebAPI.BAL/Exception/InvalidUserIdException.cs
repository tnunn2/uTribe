using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urTribeWebAPI.BAL
{
    public class InvalidUserIdException : Exception
    {
        #region Constant
        private const string MESSAGE = "Invalid userId";
        #endregion
        public InvalidUserIdException()
            : base(MESSAGE)
        {
        }

        public InvalidUserIdException(Guid eventId)
            : base(string.Format(MESSAGE+": {0} ", eventId.ToString()))
        {
        }
        public InvalidUserIdException(string message)
            : base(message)
        {
        }


        public InvalidUserIdException(Exception inner)
            : base(MESSAGE, inner)
        {
        }
    }
}
