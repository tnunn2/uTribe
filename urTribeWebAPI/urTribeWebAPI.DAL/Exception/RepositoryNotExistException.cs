using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urTribeWebAPI.DAL.Factory
{
    public class RepositoryNotExistException : Exception
    {
        #region Constant
        private const string MESSAGE = "Unable to return requested Repository.";
        #endregion
        public RepositoryNotExistException() : base(MESSAGE)
        {
        }

        public RepositoryNotExistException(Exception inner) : base(MESSAGE, inner)
        {
        }

    }
}
