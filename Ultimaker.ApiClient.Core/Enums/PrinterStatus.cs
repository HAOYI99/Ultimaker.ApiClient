namespace Ultimaker.ApiClient.Core.Enums;

public enum PrinterStatus
{
    BOOTING,
    WAITING_FOR_PERIPHERALS,
    IDLE,
    PRINTING,
    ERROR,
    MAINTENANCE,
    /// <summary>
    /// not original enum value, added to represent offline status
    /// when api is timeout
    /// </summary>
    OFFLINE
}