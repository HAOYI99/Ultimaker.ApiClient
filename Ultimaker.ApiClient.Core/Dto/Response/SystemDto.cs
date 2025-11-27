using Newtonsoft.Json;
using Ultimaker.ApiClient.Core.Dto.Response.System;
using Ultimaker.ApiClient.Core.Enums;

namespace Ultimaker.ApiClient.Core.Dto.Response;

public class SystemDto
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("platform")] public string Platform { get; set; }
    [JsonProperty("hostname")] public string Hostname { get; set; }
    [JsonProperty("firmware")] public string Firmware { get; set; }
    [JsonProperty("country")] public string Country { get; set; }
    [JsonProperty("language")] public string Language { get; set; }
    [JsonProperty("uptime")] public int Uptime { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("variant")] public PrinterVariant Variant { get; set; }
    [JsonProperty("is_country_locked")] public bool IsCountryLocked { get; set; }
    [JsonProperty("log")] public string[] Logs { get; set; }
    [JsonProperty("guid")] public Guid Id { get; set; }
    [JsonProperty("hardware")] public SystemHardware Hardware { get; set; }
    [JsonProperty("memory")] public SystemMemory Memory { get; set; }
    [JsonProperty("time")] public SystemTime Time { get; set; }
}