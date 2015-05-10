using System.Collections.Generic;
using urTribeWebAPI.Common;

namespace urTribeWebAPI.Messaging
{
    public class BrokerResult
    {
        #region Properties
        public ResultType type { get; set; }
        public ErrorReason reason { get; set; }
        public string errorMessage { get; set; }
        public List<IUser> validUsers { get; set; }
        public List<IUser> invalidUsers { get; set; }
        #endregion

        #region Public Methods
        public bool ok()
        {
            return type == ResultType.fullsuccess || type == ResultType.sufficientSuccess;
        }
        public static BrokerResult newInviteResult()
        {
            return new BrokerResult()
            {
                type = ResultType.incomplete,
                validUsers = new List<IUser>(),
                invalidUsers = new List<IUser>()
            };
        }
        #endregion

        #region Internal Methods
        internal static BrokerResult newSuccess()
        {
            return new BrokerResult() {type = ResultType.fullsuccess};
        }
        #endregion
    }
}
