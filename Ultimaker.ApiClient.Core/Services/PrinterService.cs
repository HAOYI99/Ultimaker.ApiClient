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

    public Task<PrinterDto?> Get(CancellationToken ct = default)
        => GetAsync<PrinterDto>(UltimakerPaths.Printer.Base, ct);
    
    public Task<PrinterStatus> GetStatus(CancellationToken ct = default)
        => GetAsync<PrinterStatus>(UltimakerPaths.Printer.Status, ct);
    
    public Task<LedDto?> GetLed(CancellationToken ct = default)
        => GetAsync<LedDto>(UltimakerPaths.Printer.Led, ct);
    
    public Task<decimal> GetLedHue(CancellationToken ct = default)
        => GetAsync<decimal>(UltimakerPaths.Printer.LedHue, ct);
    
    public Task<decimal> GetLedSaturation(CancellationToken ct = default)
        => GetAsync<decimal>(UltimakerPaths.Printer.LedSaturation, ct);
    
    public Task<decimal> GetLedBrightness(CancellationToken ct = default)
        => GetAsync<decimal>(UltimakerPaths.Printer.LedBrightness, ct);
    
    public Task<HeadDto[]?> GetHeads(CancellationToken ct = default)
        => GetAsync<HeadDto[]>(UltimakerPaths.Printer.Heads, ct);
    
    public Task<HeadDto?> GetHead(int headIndex, CancellationToken ct = default)
        => GetAsync<HeadDto>(UltimakerPaths.Printer.Head(headIndex), ct);
    
    public Task<DimensionDto?> GetHeadPosition(int headIndex, CancellationToken ct = default)
        => GetAsync<DimensionDto>(UltimakerPaths.Printer.HeadPosition(headIndex), ct);

    public Task<DimensionDto?> GetHeadMaxSpeed(int headIndex, CancellationToken ct = default)
        => GetAsync<DimensionDto>(UltimakerPaths.Printer.HeadMaxSpeed(headIndex), ct);
    
    public Task<DimensionDto?> GetHeadJerk(int headIndex, CancellationToken ct = default)
        => GetAsync<DimensionDto>(UltimakerPaths.Printer.HeadJerk(headIndex), ct);
    
    public Task<decimal> GetHeadAcceleration(int headIndex, CancellationToken ct = default)
        => GetAsync<decimal>(UltimakerPaths.Printer.HeadAcceleration(headIndex), ct);
    
    public Task<BedDto?> GetBed(CancellationToken ct = default)
        => GetAsync<BedDto>(UltimakerPaths.Printer.Bed, ct);

    public Task<TemperatureDto?> GetBedTemperature(CancellationToken ct = default)
        => GetAsync<TemperatureDto>(UltimakerPaths.Printer.BedTemperature, ct);
    
    public Task<PreHeatDto?> GetBedPreHeat(CancellationToken ct = default)
        => GetAsync<PreHeatDto>(UltimakerPaths.Printer.BedPreHeat, ct);
    
    public Task<string?> GetBedType(CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.Printer.BedType, ct);
    
    public Task<ExtruderDto[]?> GetExtruders(int headIndex, CancellationToken ct = default)
        => GetAsync<ExtruderDto[]>(UltimakerPaths.Printer.Extruders(headIndex), ct);
    
    public Task<ExtruderDto?> GetExtruder(int headIndex, int extruderIndex, CancellationToken ct = default)
        => GetAsync<ExtruderDto>(UltimakerPaths.Printer.Extruder(headIndex, extruderIndex), ct);
    
    public Task<ActiveMaterialDto?> GetActiveMaterial(int headIndex, int extruderIndex, CancellationToken ct = default)
        => GetAsync<ActiveMaterialDto>(UltimakerPaths.Printer.ActiveMaterial(headIndex, extruderIndex), ct);
    
    public Task<Guid> GetActiveMaterialGuid(int headIndex, int extruderIndex, CancellationToken ct = default)
        => GetAsync<Guid>(UltimakerPaths.Printer.ActiveMaterialId(headIndex, extruderIndex), ct);
    
    public Task<FeederDto?> GetFeeder(int headIndex, int extruderIndex, CancellationToken ct = default)
        => GetAsync<FeederDto>(UltimakerPaths.Printer.Feeder(headIndex, extruderIndex), ct);
    
    public Task<decimal> GetFeederJerk(int headIndex, int extruderIndex, CancellationToken ct = default)
        => GetAsync<decimal>(UltimakerPaths.Printer.FeederJerk(headIndex, extruderIndex), ct);
    
    public Task<decimal> GetFeederMaxSpeed(int headIndex, int extruderIndex, CancellationToken ct = default)
        => GetAsync<decimal>(UltimakerPaths.Printer.FeederMaxSpeed(headIndex, extruderIndex), ct);
    
    public Task<decimal> GetFeederAcceleration(int headIndex, int extruderIndex, CancellationToken ct = default)
        => GetAsync<decimal>(UltimakerPaths.Printer.FeederAcceleration(headIndex, extruderIndex), ct);
    
    public Task<HotendDto?> GetHotend(int headIndex, int extruderIndex, CancellationToken ct = default)
        => GetAsync<HotendDto>(UltimakerPaths.Printer.Hotend(headIndex, extruderIndex), ct);
    
    public Task<HotendOffsetDto?> GetHotendOffset(int headIndex, int extruderIndex, CancellationToken ct = default)
        => GetAsync<HotendOffsetDto>(UltimakerPaths.Printer.HotendOffset(headIndex, extruderIndex), ct);
    
    public Task<TemperatureDto?> GetHotendTemperature(int headIndex, int extruderIndex, CancellationToken ct = default)
        => GetAsync<TemperatureDto>(UltimakerPaths.Printer.HotendTemperature(headIndex, extruderIndex), ct);
}