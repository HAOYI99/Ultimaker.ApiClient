using Newtonsoft.Json;

namespace Ultimaker.ApiClient.Core.Dto.Response.Printer;

public class BedDto
{
     [JsonProperty("type")] public string Type { get; set; }
     [JsonProperty("temperature")] public TemperatureDto Temperature { get; set; }
     [JsonProperty("pre_heat")] public PreHeatDto PreHeat { get; set; }
}