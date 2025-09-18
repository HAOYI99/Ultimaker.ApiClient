using System.Runtime.Serialization;

namespace Ultimaker.ApiClient.Core.Enums;

public enum JobResult
{
    [EnumMember(Value = "")]
    EMPTY,
    FAILED,
    ABORTED,
    FINISHED
}