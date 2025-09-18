using Newtonsoft.Json;

namespace Ultimaker.ApiClient.Core.Dto.Response.History;

public class UsedExtruders
{
    [JsonProperty("0")] public bool FirstUsed { get; set; }
    [JsonProperty("1")] public bool SecondUsed { get; set; }
}