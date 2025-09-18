using Newtonsoft.Json;

namespace Ultimaker.ApiClient.Core.Dto.Response.Printer;

public class FeederDto
{
    [JsonProperty("acceleration")] public decimal Acceleration { get; set; }
    [JsonProperty("max_speed")] public decimal MaxSpeed { get; set; }
    [JsonProperty("jerk")] public decimal Jerk { get; set; }
}