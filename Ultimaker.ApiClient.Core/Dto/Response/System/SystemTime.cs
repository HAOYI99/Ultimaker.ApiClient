using Newtonsoft.Json;

namespace Ultimaker.ApiClient.Core.Dto.Response.System;

public class SystemTime
{
    [JsonProperty("utc")] public decimal UTC { get; set; }
}