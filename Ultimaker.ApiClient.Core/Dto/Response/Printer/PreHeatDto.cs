using Newtonsoft.Json;

namespace Ultimaker.ApiClient.Core.Dto.Response.Printer;

public class PreHeatDto
{
    [JsonProperty("active")] public bool Active { get; set; }
}