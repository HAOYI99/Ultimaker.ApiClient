using Ultimaker.ApiClient.Core.Enums;

namespace Ultimaker.ApiClient.Core.Utils;

public static class UltimakerEnumExtensions
{
    public static string ToOriString(this PrinterVariant variant)
        => $"Ultimaker {variant.ToString()}";

    public static string ToOriString(this AirManagerFilterStatus filterStatus)
        => filterStatus.ToString().ToLower();

    public static string ToOriString(this AirManagerStatus status)
        => status.ToString().ToLower();

    public static string ToOriString(this AuthStatus status)
        => status.ToString().ToLower();

    public static string ToOriString(this PrinterStatus status)
        => status.ToString().ToLower();

    public static string ToOriString(this JobResult result)
    {
        if (result == JobResult.EMPTY)
            return string.Empty;

        var stringValue = result.ToString();
        return char.ToUpper(stringValue[0]) + stringValue[1..].ToLowerInvariant();
    }
}