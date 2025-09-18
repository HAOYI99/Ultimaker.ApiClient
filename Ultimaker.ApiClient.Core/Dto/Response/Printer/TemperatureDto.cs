using Newtonsoft.Json;

namespace Ultimaker.ApiClient.Core.Dto.Response.Printer;

public class TemperatureDto
{
    [JsonProperty("target")] public decimal Target { get; set; }
    [JsonProperty("current")] public decimal Current { get; set; }
}