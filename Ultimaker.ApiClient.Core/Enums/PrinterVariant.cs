using System.Runtime.Serialization;

namespace Ultimaker.ApiClient.Core.Enums;

public enum PrinterVariant
{
    [EnumMember(Value = "Ultimaker S5")]
    S5,
    [EnumMember(Value = "Ultimaker S7")]
    S7,
    [EnumMember(Value = "Ultimaker S8")]
    S8
}