using Newtonsoft.Json;
using Ultimaker.ApiClient.Core.Dto.Response.Printer;
using Ultimaker.ApiClient.Core.Enums;

namespace Ultimaker.ApiClient.Core.Dto.Response;

public class PrinterDto
{
    [JsonProperty("status")] public PrinterStatus Status { get; set; }
    [JsonProperty("bed")] public BedDto Bed { get; set; }
    [JsonProperty("heads")] public HeadDto[] Heads { get; set; }
    [JsonProperty("led")] public LedDto Led { get; set; }
}