using System.Collections.Generic;
using urTribeWebAPI.Common.Interfaces;

namespace urTribeWebAPI.Messaging
{
    public class brokerResult
    {
        
        public ResultType type { get; set; }
        public ErrorReason reason { get; set; }
        public string errorMessage { get; set; }
        public List<IUser> validUsers { get; set; }
        public List<IUser> invalidUsers { get; set; }

        internal static brokerResult newSuccess()
        {
            return new brokerResult() {type = ResultType.fullsuccess};
        }


        public bool ok()
        {
            return type == ResultType.fullsuccess || type == ResultType.sufficientSuccess;
        }

        public static brokerResult newInviteResult()
        {
            return new brokerResult()
            {
                type = ResultType.incomplete,
                validUsers = new List<IUser>(),
                invalidUsers = new List<IUser>()
            };
        }
    }

    public enum ResultType { 
        fullsuccess, 
        sufficientSuccess, 
        respondError, 
        authError, 
        createError,
        inviteError,
        incomplete
    }
    public enum ErrorReason { 
        remoteAuthFailure,
        remoteCreateFailure
    }
}
