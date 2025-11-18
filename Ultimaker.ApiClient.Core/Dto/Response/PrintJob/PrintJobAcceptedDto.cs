using Newtonsoft.Json;

namespace Ultimaker.ApiClient.Core.Dto.Response.PrintJob;

public class PrintJobAcceptedDto
{
    [JsonProperty("message")] public string Message { get; set; }
    [JsonProperty("uuid")] public Guid JobId { get; set; }
}