using Newtonsoft.Json;
using Ultimaker.ApiClient.Core.Enums;

namespace Ultimaker.ApiClient.Core.Dto.Response;

public class CheckAuthDto
{
    [JsonProperty("message")] public AuthStatus Status { get; set; }
}