using System.Net;
using Ultimaker.ApiClient.Core.Constants;
using Ultimaker.ApiClient.Core.Dto.Response;
using Ultimaker.ApiClient.Core.Dto.Response.System;
using Ultimaker.ApiClient.Core.Enums;

namespace Ultimaker.ApiClient.Core.Services;

public class SystemService : ServiceBase
{
    public SystemService(HttpClient httpClient) : base(httpClient) { }
    public SystemService(HttpClient httpClient, NetworkCredential credential) : base(httpClient, credential) { }

    public Task<SystemDto?> Get(CancellationToken ct = default)
        => GetAsync<SystemDto>(UltimakerPaths.System.Base, ct);

    public Task<string?> GetPlatform(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.System.Platform, ct);

    public Task<string?> GetHostname(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.System.Hostname, ct);

    public Task<string?> GetFirmware(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.System.Firmware, ct);

    public Task<string?> GetFirmwareStatus(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.System.FirmwareStatus, ct);

    public Task<string?> GetFirmwareStable(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.System.FirmwareStable, ct);

    public Task<string?> GetFirmwareLatest(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.System.FirmwareLatest, ct);

    public Task<SystemMemory?> GetMemory(CancellationToken ct = default)
        => GetAsync<SystemMemory>(UltimakerPaths.System.Memory, ct);

    public Task<SystemTime?> GetTime(CancellationToken ct = default)
        => GetAsync<SystemTime>(UltimakerPaths.System.Time, ct);

    public Task<string[]?> GetLogs(int? boot, int? lines, CancellationToken ct = default)
        => GetAsync<string[]>(UltimakerPaths.System.LogQueryPath(boot, lines), ct);

    public Task<string[]?> GetLogs(CancellationToken ct = default)
        => GetLogs(null, null, ct);

    public Task<string?> GetName(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.System.Name, ct);

    public Task<string?> GetCountry(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.System.Country, ct);

    public Task<bool> GetIsCountryLocked(CancellationToken ct = default)
        => GetAsync<bool>(UltimakerPaths.System.IsCountryLocked, ct);

    public Task<string?> GetLanguage(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.System.Language, ct);

    public Task<int> GetUptime(CancellationToken ct = default)
        => GetAsync<int>(UltimakerPaths.System.Uptime, ct);

    public Task<string?> GetType(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.System.Type, ct);

    public Task<PrinterVariant> GetVariant(CancellationToken ct = default)
        => GetAsync<PrinterVariant>(UltimakerPaths.System.Variant, ct);

    public Task<SystemHardware?> GetHardware(CancellationToken ct = default)
        => GetAsync<SystemHardware>(UltimakerPaths.System.Hardware, ct);

    public Task<int> GetHardwareTypeId(CancellationToken ct = default)
        => GetAsync<int>(UltimakerPaths.System.HardwareTypeId, ct);

    public Task<int> GetHardwareRevision(CancellationToken ct = default)
        => GetAsync<int>(UltimakerPaths.System.HardwareRevision, ct);

    public Task<Guid> GetId(CancellationToken ct = default)
        => GetAsync<Guid>(UltimakerPaths.System.Guid, ct);
}