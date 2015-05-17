
namespace urTribeWebAPI.Common
{
    public enum EventAttendantsStatus { Pending, Attending, Maybe, Declined, Cancel, All};
    public enum ResultType { fullsuccess, sufficientSuccess, respondError, authError, createError, inviteError, incomplete}
    public enum ErrorReason { remoteAuthFailure, remoteCreateFailure }
    public enum UserCurrentStatus { Unavailable, Available, Away }

}
