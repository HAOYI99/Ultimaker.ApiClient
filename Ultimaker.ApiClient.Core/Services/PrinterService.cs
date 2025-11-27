using System.Net;
using Ultimaker.ApiClient.Core.Constants;
using Ultimaker.ApiClient.Core.Dto.Response;
using Ultimaker.ApiClient.Core.Dto.Response.Printer;
using Ultimaker.ApiClient.Core.Enums;

namespace Ultimaker.ApiClient.Core.Services;

public class PrinterService : ServiceBase
{
    public PrinterService(HttpClient httpClient) : base(httpClient) { }
    public PrinterService(HttpClient httpClient, NetworkCredential credential) : base(httpClient, credential) { }

    public Task<UltimakerApiResponse<PrinterDto?>> Get(CancellationToken ct = default)
        => GetAsync<PrinterDto>(UltimakerPaths.Printer.Base, ct);

    public Task<UltimakerApiResponse<PrinterStatus>> GetStatus(CancellationToken ct = default)
        => GetAsync<PrinterStatus>(UltimakerPaths.Printer.Status, ct);

    public Task<UltimakerApiResponse<LedDto?>> GetLed(CancellationToken ct = default)
        => GetAsync<LedDto>(UltimakerPaths.Printer.Led, ct);

    public Task<UltimakerApiResponse<decimal>> GetLedHue(CancellationToken ct = default)
        => GetAsync<decimal>(UltimakerPaths.Printer.LedHue, ct);

    public Task<UltimakerApiResponse<decimal>> GetLedSaturation(CancellationToken ct = default)
        => GetAsync<decimal>(UltimakerPaths.Printer.LedSaturation, ct);

    public Task<UltimakerApiResponse<decimal>> GetLedBrightness(CancellationToken ct = default)
        => GetAsync<decimal>(UltimakerPaths.Printer.LedBrightness, ct);

    public Task<UltimakerApiResponse<HeadDto[]?>> GetHeads(CancellationToken ct = default)
        => GetAsync<HeadDto[]>(UltimakerPaths.Printer.Heads, ct);

    public Task<UltimakerApiResponse<HeadDto?>> GetHead(int headIndex, CancellationToken ct = default)
        => GetAsync<HeadDto>(UltimakerPaths.Printer.Head(headIndex), ct);

    public Task<UltimakerApiResponse<DimensionDto?>> GetHeadPosition(int headIndex, CancellationToken ct = default)
        => GetAsync<DimensionDto>(UltimakerPaths.Printer.HeadPosition(headIndex), ct);

    public Task<UltimakerApiResponse<DimensionDto?>> GetHeadMaxSpeed(int headIndex, CancellationToken ct = default)
        => GetAsync<DimensionDto>(UltimakerPaths.Printer.HeadMaxSpeed(headIndex), ct);

    public Task<UltimakerApiResponse<DimensionDto?>> GetHeadJerk(int headIndex, CancellationToken ct = default)
        => GetAsync<DimensionDto>(UltimakerPaths.Printer.HeadJerk(headIndex), ct);

    public Task<UltimakerApiResponse<decimal>> GetHeadAcceleration(int headIndex, CancellationToken ct = default)
        => GetAsync<decimal>(UltimakerPaths.Printer.HeadAcceleration(headIndex), ct);

    public Task<UltimakerApiResponse<BedDto?>> GetBed(CancellationToken ct = default)
        => GetAsync<BedDto>(UltimakerPaths.Printer.Bed, ct);

    public Task<UltimakerApiResponse<TemperatureDto?>> GetBedTemperature(CancellationToken ct = default)
        => GetAsync<TemperatureDto>(UltimakerPaths.Printer.BedTemperature, ct);

    public Task<UltimakerApiResponse<PreHeatDto?>> GetBedPreHeat(CancellationToken ct = default)
        => GetAsync<PreHeatDto>(UltimakerPaths.Printer.BedPreHeat, ct);

    public Task<UltimakerApiResponse<string?>> GetBedType(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.Printer.BedType, ct);

    public Task<UltimakerApiResponse<ExtruderDto[]?>> GetExtruders(int headIndex, CancellationToken ct = default)
        => GetAsync<ExtruderDto[]>(UltimakerPaths.Printer.Extruders(headIndex), ct);

    public Task<UltimakerApiResponse<ExtruderDto?>> GetExtruder(int headIndex, int extruderIndex,
        CancellationToken ct = default)
        => GetAsync<ExtruderDto>(UltimakerPaths.Printer.Extruder(headIndex, extruderIndex), ct);

    public Task<UltimakerApiResponse<ActiveMaterialDto?>> GetActiveMaterial(int headIndex, int extruderIndex,
        CancellationToken ct = default)
        => GetAsync<ActiveMaterialDto>(UltimakerPaths.Printer.ActiveMaterial(headIndex, extruderIndex), ct);

    public Task<UltimakerApiResponse<Guid>> GetActiveMaterialGuid(int headIndex, int extruderIndex,
        CancellationToken ct = default)
        => GetAsync<Guid>(UltimakerPaths.Printer.ActiveMaterialId(headIndex, extruderIndex), ct);

    public Task<UltimakerApiResponse<FeederDto?>> GetFeeder(int headIndex, int extruderIndex,
        CancellationToken ct = default)
        => GetAsync<FeederDto>(UltimakerPaths.Printer.Feeder(headIndex, extruderIndex), ct);

    public Task<UltimakerApiResponse<decimal>> GetFeederJerk(int headIndex, int extruderIndex,
        CancellationToken ct = default)
        => GetAsync<decimal>(UltimakerPaths.Printer.FeederJerk(headIndex, extruderIndex), ct);

    public Task<UltimakerApiResponse<decimal>> GetFeederMaxSpeed(int headIndex, int extruderIndex,
        CancellationToken ct = default)
        => GetAsync<decimal>(UltimakerPaths.Printer.FeederMaxSpeed(headIndex, extruderIndex), ct);

    public Task<UltimakerApiResponse<decimal>> GetFeederAcceleration(int headIndex, int extruderIndex,
        CancellationToken ct = default)
        => GetAsync<decimal>(UltimakerPaths.Printer.FeederAcceleration(headIndex, extruderIndex), ct);

    public Task<UltimakerApiResponse<HotendDto?>> GetHotend(int headIndex, int extruderIndex,
        CancellationToken ct = default)
        => GetAsync<HotendDto>(UltimakerPaths.Printer.Hotend(headIndex, extruderIndex), ct);

    public Task<UltimakerApiResponse<HotendOffsetDto?>> GetHotendOffset(int headIndex, int extruderIndex,
        CancellationToken ct = default)
        => GetAsync<HotendOffsetDto>(UltimakerPaths.Printer.HotendOffset(headIndex, extruderIndex), ct);

    public Task<UltimakerApiResponse<TemperatureDto?>> GetHotendTemperature(int headIndex, int extruderIndex,
        CancellationToken ct = default)
        => GetAsync<TemperatureDto>(UltimakerPaths.Printer.HotendTemperature(headIndex, extruderIndex), ct);
}