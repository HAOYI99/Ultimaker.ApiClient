using System.Net;
using Ultimaker.ApiClient.Core.Constants;
using Ultimaker.ApiClient.Core.Dto;
using Ultimaker.ApiClient.Core.Dto.Response;
using Ultimaker.ApiClient.Core.Enums;
using Ultimaker.ApiClient.Core.Utils;

namespace Ultimaker.ApiClient.Core.Services;

public class PrintJobService : ServiceBase
{
    protected readonly HttpClient _longTimeoutHttpClient;

    public PrintJobService(HttpClient httpClient) : base(httpClient)
    {
        _longTimeoutHttpClient = httpClient;
        _longTimeoutHttpClient.Timeout = TimeSpan.FromMinutes(1);
    }

    public PrintJobService(HttpClient httpClient, NetworkCredential credential) : base(httpClient, credential)
    {
        _longTimeoutHttpClient = httpClient;
        _longTimeoutHttpClient.Timeout = TimeSpan.FromMinutes(1);
    }

    public Task<PrintJobDto?> Get(CancellationToken ct = default)
        => GetAsync<PrintJobDto>(UltimakerPaths.PrintJob.Base, ct);

    public Task<string?> GetName(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.PrintJob.JobName, ct);

    public Task<DateTime?> GetStartedDt(CancellationToken ct = default)
        => GetAsync<DateTime?>(UltimakerPaths.PrintJob.StartedDt, ct);

    public Task<DateTime?> GetFinishedDt(CancellationToken ct = default)
        => GetAsync<DateTime?>(UltimakerPaths.PrintJob.FinishedDt, ct);

    public Task<DateTime?> GetCleanedDt(CancellationToken ct = default)
        => GetAsync<DateTime?>(UltimakerPaths.PrintJob.CleanedDt, ct);

    public Task<Guid?> GetJobId(CancellationToken ct = default)
        => GetAsync<Guid?>(UltimakerPaths.PrintJob.JobId, ct);

    public Task<decimal?> GetProgress(CancellationToken ct = default)
        => GetAsync<decimal?>(UltimakerPaths.PrintJob.Progress, ct);

    public Task<JobState?> GetJobState(CancellationToken ct = default)
        => GetAsync<JobState?>(UltimakerPaths.PrintJob.State, ct);

    public Task<JobResult?> GetJobResult(CancellationToken ct = default)
        => GetAsync<JobResult?>(UltimakerPaths.PrintJob.Result, ct);

    public Task<int?> GetTimeElapsed(CancellationToken ct = default)
        => GetAsync<int?>(UltimakerPaths.PrintJob.TimeElapsed, ct);

    public Task<int?> GetTimeTotal(CancellationToken ct = default)
        => GetAsync<int?>(UltimakerPaths.PrintJob.TimeTotal, ct);

    public Task<string?> GetSource(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.PrintJob.Source, ct);

    public Task<string?> GetSourceUser(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.PrintJob.SourceUser, ct);

    public Task<string?> GetSourceApp(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.PrintJob.SourceApp, ct);

    public Task<string?> GetPauseSrc(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.PrintJob.PauseSrc, ct);

    public Task<Guid?> GetReprintOriginalUuid(CancellationToken ct = default)
        => GetAsync<Guid?>(UltimakerPaths.PrintJob.ReprintOriginalUuid, ct);

    public Task<string?> GetGCode(CancellationToken ct = default)
        => GetGCodeAsync(UltimakerPaths.PrintJob.GCode, ct);

    /// <summary>
    /// This method returns the ufp file as a byte array.
    /// </summary>
    public Task<FileItem?> GetContainer(CancellationToken ct = default)
        => GetFileAsync(UltimakerPaths.PrintJob.Container, ct);

    private async Task<string?> GetGCodeAsync(string path, CancellationToken ct = default)
    {
        EnsureHasCredential();
        var response = await _longTimeoutHttpClient.GetAsync(path, ct);
        if (response.IsNotFound())
            return null;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(ct);
    }

    private async Task<FileItem?> GetFileAsync(string path, CancellationToken ct = default)
    {
        EnsureHasCredential();
        var response = await _httpClient.GetAsync(path, ct);
        if (response.IsNotFound())
            return null;
        response.EnsureSuccessStatusCode();
        var contents = await response.Content.ReadAsByteArrayAsync(ct);
        var filename = response.Content.Headers.ContentDisposition?.FileName?.Trim('"') ?? "undefined_filename";
        return new FileItem(contents, filename);
    }
}