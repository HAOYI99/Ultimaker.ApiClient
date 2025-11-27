using System.Runtime.Serialization;

namespace Ultimaker.ApiClient.Core.Enums;

public enum UpdateJobStateOpt
{
    [EnumMember(Value = "print")]
    PRINT,
    [EnumMember(Value = "pause")]
    PAUSE,
    [EnumMember(Value = "abort")]
    ABORT,
}