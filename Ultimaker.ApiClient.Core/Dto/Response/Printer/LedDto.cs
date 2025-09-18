using Newtonsoft.Json;

namespace Ultimaker.ApiClient.Core.Dto.Response.Printer;

public class LedDto
{
    [JsonProperty("hue")] public decimal Hue { get; set; }
    [JsonProperty("saturation")] public decimal Saturation { get; set; }
    [JsonProperty("brightness")] public decimal Brightness { get; set; }
}