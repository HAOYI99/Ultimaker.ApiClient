using Newtonsoft.Json;

namespace Ultimaker.ApiClient.Core.Dto.Response.System;

public class SystemMemory
{
    [JsonProperty("total")] public int Total { get; set; }
    [JsonProperty("used")] public int Used { get; set; }
}