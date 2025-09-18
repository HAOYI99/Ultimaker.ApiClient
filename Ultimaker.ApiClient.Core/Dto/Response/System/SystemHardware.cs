using Newtonsoft.Json;

namespace Ultimaker.ApiClient.Core.Dto.Response.System;

public class SystemHardware
{
    [JsonProperty("revision")] public int Revision { get; set; }
    [JsonProperty("typeid")] public int TypeId { get; set; }
}