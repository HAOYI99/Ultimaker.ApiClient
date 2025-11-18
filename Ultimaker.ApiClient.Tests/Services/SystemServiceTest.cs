using System.Net;
using System.Net.Mime;
using JetBrains.Annotations;
using RichardSzalay.MockHttp;
using Ultimaker.ApiClient.Core.Constants;
using Ultimaker.ApiClient.Core.Enums;
using Ultimaker.ApiClient.Core.Services;

namespace Ultimaker.ApiClient.Tests.Services;

[TestSubject(typeof(SystemService))]
public class SystemServiceTest
{
    private MockHttpMessageHandler _mockHttp;
    private SystemService _service;
    private const string BaseUrl = "http://localhost:8080";

    public SystemServiceTest()
    {
        _mockHttp = new MockHttpMessageHandler();
        var httpClient = _mockHttp.ToHttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(0.5);
        httpClient.BaseAddress = new Uri(BaseUrl);
        _service = new SystemService(httpClient);
    }

    [Fact]
    public async Task GetSystem()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.System.Base}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                {
                  "country": "",
                  "display_message": {},
                  "firmware": "8.2.0",
                  "guid": "ad5c572f-8f40-4170-9b6b-273c0b5616bc",
                  "hardware": {
                    "revision": 2,
                    "typeid": 5078167
                  },
                  "hostname": "localhost:8080",
                  "is_country_locked": false,
                  "language": "en",
                  "log": [
                      "just a log line"
                  ],
                  "memory": {
                    "total": 1053614080,
                    "used": 583208960
                  },
                  "name": "PrinterName",
                  "platform": "Linux-4.14.32-ultimaker+-armv7l-with-debian-10.1",
                  "time": {
                    "utc": 1758851833.8009684
                  },
                  "type": "3D printer",
                  "uptime": 1213056,
                  "variant": "Ultimaker S7"
                }
                """);
        var result = await _service.Get();
        Assert.NotNull(result);
        Assert.Equal("8.2.0", result.Firmware);
        Assert.Equal("ad5c572f-8f40-4170-9b6b-273c0b5616bc", result.Id.ToString());
        Assert.Equal(5078167, result.Hardware.TypeId);
        Assert.Equal(2, result.Hardware.Revision);
        Assert.Equal("localhost:8080", result.Hostname);
        Assert.False(result.IsCountryLocked);
        Assert.Equal("en", result.Language);
        Assert.Single(result.Logs);
        Assert.Equal(1053614080, result.Memory.Total);
        Assert.Equal(583208960, result.Memory.Used);
        Assert.Equal("PrinterName", result.Name);
        Assert.Equal("Linux-4.14.32-ultimaker+-armv7l-with-debian-10.1", result.Platform);
        Assert.Equal(1758851833.8009684m, result.Time.UTC);
        Assert.Equal("3D printer", result.Type);
        Assert.Equal(1213056, result.Uptime);
        Assert.Equal(PrinterVariant.S7, result.Variant);
    }

    [Fact]
    public async Task GetPlatform()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.System.Platform}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "Linux-4.14.32-ultimaker+-armv7l-with-debian-10.1"
                """);
        var result = await _service.GetPlatform();
        Assert.NotNull(result);
        Assert.Equal("Linux-4.14.32-ultimaker+-armv7l-with-debian-10.1", result);
    }

    [Fact]
    public async Task GetHostname()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.System.Hostname}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "localhost:8080"
                """);
        var result = await _service.GetHostname();
        Assert.NotNull(result);
        Assert.Equal("localhost:8080", result);
    }

    [Fact]
    public async Task GetFirmware()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.System.Firmware}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "8.2.0"
                """);
        var result = await _service.GetFirmware();
        Assert.NotNull(result);
        Assert.Equal("8.2.0", result);
    }

    [Fact]
    public async Task GetFirmwareStatus()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.System.FirmwareStatus}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "IDLE"
                """);
        var result = await _service.GetFirmwareStatus();
        Assert.NotNull(result);
        Assert.Equal("IDLE", result);
    }

    [Fact]
    public async Task GetFirmwareStable()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.System.FirmwareStable}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "8.2.0"
                """);
        var result = await _service.GetFirmwareStable();
        Assert.NotNull(result);
        Assert.Equal("8.2.0", result);
    }

    [Fact]
    public async Task GetFirmwareLatest()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.System.FirmwareLatest}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "8.2.0"
                """);
        var result = await _service.GetFirmwareLatest();
        Assert.NotNull(result);
        Assert.Equal("8.2.0", result);
    }

    [Fact]
    public async Task GetMemory()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.System.Memory}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                    {
                      "total": 1053614080,
                      "used": 583208960 
                    }
                """);
        var result = await _service.GetMemory();
        Assert.NotNull(result);
        Assert.Equal(1053614080, result.Total);
        Assert.Equal(583208960, result.Used);
    }

    [Fact]
    public async Task GetTime()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.System.Time}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                {
                  "utc": 1758851833.8009684
                }
                """);
        var result = await _service.GetTime();
        Assert.NotNull(result);
        Assert.Equal(1758851833.8009684m, result.UTC);
    }

    [Fact]
    public async Task GetLogs()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.System.Logs}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                [
                    "just a log line"
                ]
                """);
        var result = await _service.GetLogs();
        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetLogs_WithParams()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.System.LogQueryPath(0, 1)}")
            .WithExactQueryString("boot=0&lines=1")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                [
                    "just a log line",
                ]
                """);
        var result = await _service.GetLogs(0, 1);
        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetName()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.System.Name}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "PrinterName"
                """);
        var result = await _service.GetName();
        Assert.NotNull(result);
        Assert.Equal("PrinterName", result);
    }

    [Fact]
    public async Task GetCountry()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.System.Country}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "US"
                """);
        var result = await _service.GetCountry();
        Assert.NotNull(result);
        Assert.Equal("US", result);
    }

    [Fact]
    public async Task GetIsCountryLocked()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.System.IsCountryLocked}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                "false");
        var result = await _service.GetIsCountryLocked();
        Assert.False(result);
    }

    [Fact]
    public async Task GetLanguage()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.System.Language}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "en"
                """);
        var result = await _service.GetLanguage();
        Assert.NotNull(result);
        Assert.Equal("en", result);
    }

    [Fact]
    public async Task GetUptime()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.System.Uptime}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                "1213056");
        var result = await _service.GetUptime();
        Assert.Equal(1213056, result);
    }

    [Fact]
    public async Task GetSystemType()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.System.Type}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "3D printer"
                """);
        var result = await _service.GetType();
        Assert.NotNull(result);
        Assert.Equal("3D printer", result);
    }

    [Fact]
    public async Task GetVariant()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.System.Variant}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "Ultimaker S7"
                """);
        var result = await _service.GetVariant();
        Assert.Equal(PrinterVariant.S7, result);
    }

    [Fact]
    public async Task GetHardware()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.System.Hardware}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                {
                    "revision": 2,
                    "typeid": 5078167
                }
                """);
        var result = await _service.GetHardware();
        Assert.NotNull(result);
        Assert.Equal(5078167, result.TypeId);
        Assert.Equal(2, result.Revision);
    }

    [Fact]
    public async Task GetHardwareTypeId()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.System.HardwareTypeId}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                "5078167");
        var result = await _service.GetHardwareTypeId();
        Assert.Equal(5078167, result);
    }

    [Fact]
    public async Task GetHardwareRevision()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.System.HardwareRevision}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                "2");
        var result = await _service.GetHardwareRevision();
        Assert.Equal(2, result);
    }

    [Fact]
    public async Task GetId()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.System.Guid}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "ad5c572f-8f40-4170-9b6b-273c0b5616bc"
                """);
        var result = await _service.GetId();
        Assert.Equal("ad5c572f-8f40-4170-9b6b-273c0b5616bc", result.ToString());
    }
}