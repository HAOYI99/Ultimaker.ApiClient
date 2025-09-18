using Newtonsoft.Json;

namespace Ultimaker.ApiClient.Core.Dto.Response;

public class ResultDto
{
    [JsonProperty("message")] public string Message { get; set; }
    [JsonProperty("result")] public bool Success { get; set; }
}