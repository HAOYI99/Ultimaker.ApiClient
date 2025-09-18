using Newtonsoft.Json;

namespace Ultimaker.ApiClient.Core.Dto.Response.Printer;

public class HotendStatisticsDto
{
    [JsonProperty("last_material_guid")] public Guid LastMaterialGuid { get; set; }
    [JsonProperty("material_extruded")] public int MaterialExtruded { get; set; }
    [JsonProperty("max_temperature_exposed")] public int MaxTemperatureExposed { get; set; }
    [JsonProperty("prints_since_cleaned")] public int PrintsSinceCleaned { get; set; }
    [JsonProperty("time_spent_hot")] public int TimeSpentHot { get; set; }
}