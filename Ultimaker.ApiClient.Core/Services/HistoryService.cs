using System.Net;
using Ultimaker.ApiClient.Core.Constants;
using Ultimaker.ApiClient.Core.Dto.Response;

namespace Ultimaker.ApiClient.Core.Services;

public class HistoryService : ServiceBase
{
    public HistoryService(HttpClient httpClient) : base(httpClient) { }
    public HistoryService(HttpClient httpClient, NetworkCredential credential) : base(httpClient, credential) { }

    public Task<UltimakerApiResponse<HistoryPrintJobDto[]?>> GetPrintJobs(int? offset, int? count, CancellationToken ct = default)
        => GetAsync<HistoryPrintJobDto[]>(UltimakerPaths.History.PrintJobQueryPath(offset, count), ct);

    public Task<UltimakerApiResponse<HistoryPrintJobDto[]?>> GetPrintJobs(CancellationToken ct = default)
        => GetPrintJobs(null, null, ct);

    public Task<UltimakerApiResponse<HistoryPrintJobDto?>> GetPrintJobById(Guid jobId, CancellationToken ct = default)
        => GetAsync<HistoryPrintJobDto>(UltimakerPaths.History.PrintJobIdPath(jobId), ct);

    public Task<UltimakerApiResponse<HistoryEventDto[]?>> GetEvents(int? offset, int? count, int? typeId, CancellationToken ct = default)
        => GetAsync<HistoryEventDto[]>(UltimakerPaths.History.EventsQueryPath(offset, count, typeId), ct);
    
    public Task<UltimakerApiResponse<HistoryEventDto[]?>> GetEvents(CancellationToken ct = default)
        => GetEvents(null, null, null, ct);
}