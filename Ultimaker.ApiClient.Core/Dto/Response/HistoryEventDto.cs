using Newtonsoft.Json;

namespace Ultimaker.ApiClient.Core.Dto.Response;

public class HistoryEventDto
{
    [JsonProperty("message")] public string Message { get; set; }
    [JsonProperty("parameters")] public string[] Parameters { get; set; }
    [JsonProperty("time")] public DateTime Time { get; set; }
    [JsonProperty("type_id")] public int TypeId { get; set; }
}