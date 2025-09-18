using Newtonsoft.Json;

namespace Ultimaker.ApiClient.Core.Dto.Response;

public class AuthDto
{
    [JsonProperty("id")] public string AuthId { get; set; }
    [JsonProperty("key")] public string AuthKey { get; set; }
}