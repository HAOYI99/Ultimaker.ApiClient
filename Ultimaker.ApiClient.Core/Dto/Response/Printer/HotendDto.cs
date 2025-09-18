using Newtonsoft.Json;

namespace Ultimaker.ApiClient.Core.Dto.Response.Printer;

public class HotendDto
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("serial")] public string Serial { get; set; }
    [JsonProperty("revision")] public string Revision { get; set; }
    [JsonProperty("temperature")] public TemperatureDto Temperature { get; set; }
    [JsonProperty("offset")] public HotendOffsetDto Offset { get; set; }
    [JsonProperty("statistics")] public HotendStatisticsDto Statistics { get; set; }
}