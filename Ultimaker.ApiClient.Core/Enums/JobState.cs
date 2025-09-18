namespace Ultimaker.ApiClient.Core.Enums;

public enum JobState
{
    NONE,
    PRINTING,
    PAUSING,
    PAUSED,
    RESUMING,
    PRE_PRINT,
    POST_PRINT,
    WAIT_CLEANUP,
    WAIT_USER_ACTION
}