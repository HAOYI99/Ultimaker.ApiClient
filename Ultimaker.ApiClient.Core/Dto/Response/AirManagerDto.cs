using Newtonsoft.Json;
using Ultimaker.ApiClient.Core.Enums;

namespace Ultimaker.ApiClient.Core.Dto.Response;

public class AirManagerDto
{
    [JsonProperty("ambient_temperature")] public decimal AmbientTemperature { get; set; }
    [JsonProperty("exhaust_temperature")] public decimal ExhaustTemperature { get; set; }
    [JsonProperty("fan_speed")] public int FanSpeed { get; set; }
    [JsonProperty("filter_age")] public int FilterAge { get; set; }
    [JsonProperty("filter_max_age")] public decimal FilterMaxAge { get; set; }
    [JsonProperty("filter_status")] public AirManagerFilterStatus FilterStatus { get; set; }
    [JsonProperty("firmware_version")] public string FirmwareVersion { get; set; }
    [JsonProperty("status")] public AirManagerStatus Status { get; set; }
}