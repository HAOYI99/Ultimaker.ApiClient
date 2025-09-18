using Newtonsoft.Json;

namespace Ultimaker.ApiClient.Core.Dto.Response.Printer;

public class HeadDto
{
     [JsonProperty("acceleration")]  public decimal Acceleration { get; set; }
     [JsonProperty("fan")] public decimal Fan { get; set; }
     [JsonProperty("position")] public DimensionDto Position { get; set; }
     [JsonProperty("max_speed")] public DimensionDto MaxSpeed { get; set; }
     [JsonProperty("jerk")] public DimensionDto Jerk { get; set; }
     [JsonProperty("extruders")] public ExtruderDto[] Extruders { get; set; }
}