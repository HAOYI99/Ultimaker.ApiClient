using Newtonsoft.Json;
using Ultimaker.ApiClient.Core.Enums;

namespace Ultimaker.ApiClient.Core.Dto.Response;

public class PrintJobDto
{
    [JsonProperty("datetime_cleaned")] public DateTime? CleanedDt { get; set; }
    [JsonProperty("datetime_finished")] public DateTime? FinishedDt { get; set; }
    [JsonProperty("datetime_started")] public DateTime StartedDt { get; set; }
    [JsonProperty("name")] public string JobName { get; set; }
    [JsonProperty("pause_source")] public string PauseSrc { get; set; }
    [JsonProperty("progress")] public decimal Progress { get; set; }
    [JsonProperty("reprint_original_uuid")] public Guid? ReprintOriginalUuid { get; set; }
    [JsonProperty("result")] public JobResult Result { get; set; }
    [JsonProperty("source")] public string Source { get; set; }
    [JsonProperty("source_application")] public string SourceApp { get; set; }
    [JsonProperty("source_user")] public string SourceUser { get; set; }
    [JsonProperty("state")] public JobState State { get; set; }
    [JsonProperty("time_elapsed")] public int TimeElapsed { get; set; }
    [JsonProperty("time_total")] public int TimeTotal { get; set; }
    [JsonProperty("uuid")] public Guid JobId { get; set; }
}