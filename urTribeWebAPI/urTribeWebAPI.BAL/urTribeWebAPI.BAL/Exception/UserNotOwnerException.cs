using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urTribeWebAPI.BAL
{
    public class UserNotOwnerException : Exception
    {
        #region Constant
        private const string MESSAGE = "User is not the owner of the Event.";
        #endregion
        public UserNotOwnerException() : base(MESSAGE)
        {
        }

        public UserNotOwnerException(string message)
            : base(message)
        {
        }


        public UserNotOwnerException(Exception inner)
            : base(MESSAGE, inner)
        {
        }

    }
}
