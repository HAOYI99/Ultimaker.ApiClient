using Newtonsoft.Json;
using Ultimaker.ApiClient.Core.Enums;

namespace Ultimaker.ApiClient.Core.Dto.Response.Printer;

public class HotendOffsetDto : DimensionDto
{
    [JsonProperty("state")] public Validity State { get; set; }
}