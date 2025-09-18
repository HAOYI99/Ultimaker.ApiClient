using Newtonsoft.Json;

namespace Ultimaker.ApiClient.Core.Dto.Response.Printer;

public class ActiveMaterialDto
{
    [JsonProperty("guid")] public Guid? Id { get; set; }
}