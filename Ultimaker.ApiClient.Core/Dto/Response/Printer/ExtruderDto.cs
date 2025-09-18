using Newtonsoft.Json;

namespace Ultimaker.ApiClient.Core.Dto.Response.Printer;

public class ExtruderDto
{
    [JsonProperty("active_material")] public ActiveMaterialDto ActiveMaterial { get; set; }
    [JsonProperty("feeder")] public FeederDto Feeder { get; set; }
    [JsonProperty("hotend")] public HotendDto Hotend { get; set; }
}