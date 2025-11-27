using Newtonsoft.Json;
using Ultimaker.ApiClient.Core.Enums;

namespace Ultimaker.ApiClient.Core.Dto.Request;

public class UpdateJobStateDto
{
    [JsonProperty("target")] public UpdateJobStateOpt NewState { get; set; }
}