using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urTribeWebAPI.BAL
{
    class RelationshipException : Exception
    {
        #region Constant
        private const string MESSAGE = "There is no relationship between the two entities.";
        #endregion
        public RelationshipException() : base(MESSAGE)
        {
        }

        public RelationshipException(Exception inner)
            : base(MESSAGE, inner)
        {
        }
    }
}
