using System.Net;
using Newtonsoft.Json;
using Ultimaker.ApiClient.Core.Constants;
using Ultimaker.ApiClient.Core.Dto;
using Ultimaker.ApiClient.Core.Dto.Request;
using Ultimaker.ApiClient.Core.Dto.Response;
using Ultimaker.ApiClient.Core.Dto.Response.PrintJob;
using Ultimaker.ApiClient.Core.Enums;
using Ultimaker.ApiClient.Core.Helper;
using Ultimaker.ApiClient.Core.Utils;

namespace Ultimaker.ApiClient.Core.Services;

public class PrintJobService : ServiceBase
{
    private readonly HttpClient _longTimeoutHttpClient;

    public PrintJobService(HttpClient httpClient) : base(httpClient)
    {
        _longTimeoutHttpClient = httpClient;
        if (httpClient.Timeout < TimeSpan.FromMinutes(1))
            _longTimeoutHttpClient.Timeout = TimeSpan.FromMinutes(1);
    }

    public PrintJobService(HttpClient httpClient, NetworkCredential credential) : base(httpClient, credential)
    {
        _longTimeoutHttpClient = httpClient;
        if (httpClient.Timeout < TimeSpan.FromMinutes(1))
            _longTimeoutHttpClient.Timeout = TimeSpan.FromMinutes(1);
    }

    public Task<UltimakerApiResponse<PrintJobDto?>> Get(CancellationToken ct = default)
        => GetAsync<PrintJobDto>(UltimakerPaths.PrintJob.Base, ct);

    public Task<UltimakerApiResponse<PrintJobAcceptedDto?>> Start(FileItem printFile, CancellationToken ct = default)
        => StartPrintJobAsync(UltimakerPaths.PrintJob.Base, printFile, ct);

    public Task<UltimakerApiResponse<string?>> GetName(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.PrintJob.JobName, ct);

    public Task<UltimakerApiResponse<DateTime?>> GetStartedDt(CancellationToken ct = default)
        => GetAsync<DateTime?>(UltimakerPaths.PrintJob.StartedDt, ct);

    public Task<UltimakerApiResponse<DateTime?>> GetFinishedDt(CancellationToken ct = default)
        => GetAsync<DateTime?>(UltimakerPaths.PrintJob.FinishedDt, ct);

    public Task<UltimakerApiResponse<DateTime?>> GetCleanedDt(CancellationToken ct = default)
        => GetAsync<DateTime?>(UltimakerPaths.PrintJob.CleanedDt, ct);

    public Task<UltimakerApiResponse<Guid?>> GetJobId(CancellationToken ct = default)
        => GetAsync<Guid?>(UltimakerPaths.PrintJob.JobId, ct);

    public Task<UltimakerApiResponse<decimal?>> GetProgress(CancellationToken ct = default)
        => GetAsync<decimal?>(UltimakerPaths.PrintJob.Progress, ct);

    public Task<UltimakerApiResponse<JobState?>> GetJobState(CancellationToken ct = default)
        => GetAsync<JobState?>(UltimakerPaths.PrintJob.State, ct);

    /// <summary>
    /// set a new job state
    /// but note that not all state transitions are valid, it will always return true
    /// make sure to check the job state before and after calling this method
    /// </summary>
    public Task<UltimakerApiResponse<HttpStatusCode>> SetJobState(UpdateJobStateOpt newState,
        CancellationToken ct = default)
        => PutJobStateAsync(UltimakerPaths.PrintJob.State, newState, ct);

    public Task<UltimakerApiResponse<JobResult?>> GetJobResult(CancellationToken ct = default)
        => GetAsync<JobResult?>(UltimakerPaths.PrintJob.Result, ct);

    public Task<UltimakerApiResponse<int?>> GetTimeElapsed(CancellationToken ct = default)
        => GetAsync<int?>(UltimakerPaths.PrintJob.TimeElapsed, ct);

    public Task<UltimakerApiResponse<int?>> GetTimeTotal(CancellationToken ct = default)
        => GetAsync<int?>(UltimakerPaths.PrintJob.TimeTotal, ct);

    public Task<UltimakerApiResponse<string?>> GetSource(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.PrintJob.Source, ct);

    public Task<UltimakerApiResponse<string?>> GetSourceUser(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.PrintJob.SourceUser, ct);

    public Task<UltimakerApiResponse<string?>> GetSourceApp(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.PrintJob.SourceApp, ct);

    public Task<UltimakerApiResponse<string?>> GetPauseSrc(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.PrintJob.PauseSrc, ct);

    public Task<UltimakerApiResponse<Guid?>> GetReprintOriginalUuid(CancellationToken ct = default)
        => GetAsync<Guid?>(UltimakerPaths.PrintJob.ReprintOriginalUuid, ct);

    public Task<UltimakerApiResponse<string?>> GetGCode(CancellationToken ct = default)
        => GetGCodeAsync(UltimakerPaths.PrintJob.GCode, ct);

    /// <summary>
    /// This method returns the ufp file as a byte array.
    /// </summary>
    public Task<UltimakerApiResponse<FileItem?>> GetContainer(CancellationToken ct = default)
        => GetFileAsync(UltimakerPaths.PrintJob.Container, ct);

    private async Task<UltimakerApiResponse<HttpStatusCode>> PutJobStateAsync(string path, UpdateJobStateOpt newState,
        CancellationToken ct = default)
    {
        EnsureHasCredential();
        var requestBody = new UpdateJobStateDto { NewState = newState };
        var requestContent = new StringJsonContent(requestBody, _jsonSetting);
        var response = await _httpClient.PutAsync(path, requestContent, ct);
        return new UltimakerApiResponse<HttpStatusCode>(response, response.StatusCode);
    }

    private async Task<UltimakerApiResponse<PrintJobAcceptedDto?>> StartPrintJobAsync(string path, FileItem printFile,
        CancellationToken ct = default)
    {
        EnsureHasCredential();
        var formData = new MultipartFormBuilder()
            .AddFile("file", printFile)
            .AddString("jobname", printFile.FileNameOnly)
            .Build();
        return await PostAsync<PrintJobAcceptedDto>(path, formData, ct);
    }

    private async Task<UltimakerApiResponse<string?>> GetGCodeAsync(string path, CancellationToken ct = default)
    {
        EnsureHasCredential();
        var response = await _longTimeoutHttpClient.GetAsync(path, ct);
        if (response.IsNotFound())
            return new UltimakerApiResponse<string?>(response);
        response.EnsureSuccessStatusCode();
        var gcode = await response.Content.ReadAsStringAsync(ct);
        return new UltimakerApiResponse<string?>(response, data: gcode);
    }

    private async Task<UltimakerApiResponse<FileItem?>> GetFileAsync(string path, CancellationToken ct = default)
    {
        EnsureHasCredential();
        var response = await _httpClient.GetAsync(path, ct);
        if (response.IsNotFound())
            return new UltimakerApiResponse<FileItem?>(response);
        response.EnsureSuccessStatusCode();
        var contents = await response.Content.ReadAsByteArrayAsync(ct);
        var filename = response.Content.Headers.ContentDisposition?.FileName?.Trim('"') ?? "undefined_filename";
        var file = new FileItem(contents, filename);
        return new UltimakerApiResponse<FileItem?>(response, file);
    }
}