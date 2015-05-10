
namespace urTribeWebAPI.Common
{
    public enum EventAttendantsStatus { Pending, Attending, Declined, Cancel, All};
    public enum ResultType { fullsuccess, sufficientSuccess, respondError, authError, createError, inviteError, incomplete }
    public enum ErrorReason { remoteAuthFailure, remoteCreateFailure }

}
