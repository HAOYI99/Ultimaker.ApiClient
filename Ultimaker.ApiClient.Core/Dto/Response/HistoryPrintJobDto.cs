using Newtonsoft.Json;
using Ultimaker.ApiClient.Core.Dto.Response.History;
using Ultimaker.ApiClient.Core.Enums;

namespace Ultimaker.ApiClient.Core.Dto.Response;

public class HistoryPrintJobDto
{
    [JsonProperty("datetime_cleaned")] public DateTime? CleanedDt { get; set; }
    [JsonProperty("datetime_finished")] public DateTime? FinishedDt { get; set; }
    [JsonProperty("datetime_started")] public DateTime StartedDt { get; set; }
    [JsonProperty("extruders_used")] public UsedExtruders UsedExtruders { get; set; }
    [JsonProperty("interrupted_step")] public string? InterruptedStep { get; set; }
    [JsonProperty("material_0_guid")] public Guid? FirstMaterialGuid { get; set; }
    [JsonProperty("material_1_guid")] public Guid? SecondMaterialGuid { get; set; }
    [JsonProperty("name")] public string JobName { get; set; }
    [JsonProperty("printcore_0_name")] public string? FirstPrintCoreName { get; set; }
    [JsonProperty("printcore_0_revision")] public string? FirstPrintCoreRevision { get; set; }
    [JsonProperty("printcore_1_name")] public string? SecondPrintCoreName { get; set; }
    [JsonProperty("printcore_1_revision")] public string? SecondPrintCoreRevision { get; set; }
    [JsonProperty("reprint_original_uuid")] public Guid? ReprintOriginalUuid { get; set; }
    [JsonProperty("result")] public JobResult? Result { get; set; }
    [JsonProperty("source")] public string Source { get; set; }
    [JsonProperty("time_elapsed")] public decimal TimeElapsed { get; set; }
    [JsonProperty("time_estimated")] public decimal TimeEstimated { get; set; }
    [JsonProperty("time_total")] public decimal TimeTotal { get; set; }
    [JsonProperty("uuid")] public Guid JobId { get; set; }
}