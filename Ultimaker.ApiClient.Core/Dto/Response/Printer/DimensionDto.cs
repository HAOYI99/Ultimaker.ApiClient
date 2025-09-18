using Newtonsoft.Json;

namespace Ultimaker.ApiClient.Core.Dto.Response.Printer;

public class DimensionDto
{
    [JsonProperty("x")] public decimal X { get; set; }
    [JsonProperty("y")] public decimal Y { get; set; }
    [JsonProperty("z")] public decimal Z { get; set; }
}