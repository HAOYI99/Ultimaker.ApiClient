using System.Net;
using Ultimaker.ApiClient.Core.Constants;
using Ultimaker.ApiClient.Core.Dto.Response;
using Ultimaker.ApiClient.Core.Dto.Response.System;
using Ultimaker.ApiClient.Core.Enums;
using Ultimaker.ApiClient.Core.Helper;

namespace Ultimaker.ApiClient.Core.Services;

public class SystemService : ServiceBase
{
    public SystemService(HttpClient httpClient) : base(httpClient) { }
    public SystemService(HttpClient httpClient, NetworkCredential credential) : base(httpClient, credential) { }

    public Task<UltimakerApiResponse<SystemDto?>> Get(CancellationToken ct = default)
        => GetAsync<SystemDto>(UltimakerPaths.System.Base, ct);

    public Task<UltimakerApiResponse<string?>> GetPlatform(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.System.Platform, ct);

    public Task<UltimakerApiResponse<string?>> GetHostname(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.System.Hostname, ct);

    public Task<UltimakerApiResponse<string?>> GetFirmware(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.System.Firmware, ct);

    public Task<UltimakerApiResponse<string?>> GetFirmwareStatus(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.System.FirmwareStatus, ct);

    public Task<UltimakerApiResponse<string?>> GetFirmwareStable(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.System.FirmwareStable, ct);

    public Task<UltimakerApiResponse<string?>> GetFirmwareLatest(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.System.FirmwareLatest, ct);

    public Task<UltimakerApiResponse<SystemMemory?>> GetMemory(CancellationToken ct = default)
        => GetAsync<SystemMemory>(UltimakerPaths.System.Memory, ct);

    public Task<UltimakerApiResponse<SystemTime?>> GetTime(CancellationToken ct = default)
        => GetAsync<SystemTime>(UltimakerPaths.System.Time, ct);

    public Task<UltimakerApiResponse<string[]?>> GetLogs(int? boot, int? lines, CancellationToken ct = default)
        => GetAsync<string[]>(UltimakerPaths.System.LogQueryPath(boot, lines), ct);

    public Task<UltimakerApiResponse<string[]?>> GetLogs(CancellationToken ct = default)
        => GetLogs(null, null, ct);

    public Task<UltimakerApiResponse<string?>> GetName(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.System.Name, ct);

    public Task<UltimakerApiResponse<HttpStatusCode>> SetName(string newName, CancellationToken ct = default)
        => UpdateNameAsync(UltimakerPaths.System.Name, newName, ct);

    public Task<UltimakerApiResponse<string?>> GetCountry(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.System.Country, ct);

    public Task<UltimakerApiResponse<bool>> GetIsCountryLocked(CancellationToken ct = default)
        => GetAsync<bool>(UltimakerPaths.System.IsCountryLocked, ct);

    public Task<UltimakerApiResponse<string?>> GetLanguage(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.System.Language, ct);

    public Task<UltimakerApiResponse<int>> GetUptime(CancellationToken ct = default)
        => GetAsync<int>(UltimakerPaths.System.Uptime, ct);

    public Task<UltimakerApiResponse<string?>> GetType(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.System.Type, ct);

    public Task<UltimakerApiResponse<PrinterVariant>> GetVariant(CancellationToken ct = default)
        => GetAsync<PrinterVariant>(UltimakerPaths.System.Variant, ct);

    public Task<UltimakerApiResponse<SystemHardware?>> GetHardware(CancellationToken ct = default)
        => GetAsync<SystemHardware>(UltimakerPaths.System.Hardware, ct);

    public Task<UltimakerApiResponse<int>> GetHardwareTypeId(CancellationToken ct = default)
        => GetAsync<int>(UltimakerPaths.System.HardwareTypeId, ct);

    public Task<UltimakerApiResponse<int>> GetHardwareRevision(CancellationToken ct = default)
        => GetAsync<int>(UltimakerPaths.System.HardwareRevision, ct);

    public Task<UltimakerApiResponse<Guid>> GetId(CancellationToken ct = default)
        => GetAsync<Guid>(UltimakerPaths.System.Guid, ct);

    private async Task<UltimakerApiResponse<HttpStatusCode>> UpdateNameAsync(string path, string newName,
        CancellationToken ct = default)
    {
        EnsureHasCredential();
        var requestContent = new StringJsonContent(newName);
        var response = await _httpClient.PutAsync(path, requestContent, ct);
        return new UltimakerApiResponse<HttpStatusCode>(response, response.StatusCode);
    }
}