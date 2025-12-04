using System.Net;
using System.Net.Mime;
using JetBrains.Annotations;
using RichardSzalay.MockHttp;
using Ultimaker.ApiClient.Core.Constants;
using Ultimaker.ApiClient.Core.Dto.Response.System;
using Ultimaker.ApiClient.Core.Enums;
using Ultimaker.ApiClient.Core.Services;

namespace Ultimaker.ApiClient.Tests.Services;

[TestSubject(typeof(SystemService))]
public class SystemServiceTest
{
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly SystemService _service;
    private const string BaseUrl = "http://localhost:8080";

    public SystemServiceTest()
    {
        _mockHttp = new MockHttpMessageHandler();
        var httpClient = _mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri(BaseUrl);
        // We need credentials for SetName
        var credential = new NetworkCredential("user", "pass");
        _service = new SystemService(httpClient, credential);
    }

    [Fact]
    public async Task Get()
    {
        var json = """{"name": "MyPrinter", "platform": "Linux"}""";
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.System.Base}")
            .Respond("application/json", json);
        var result = await _service.Get();
        Assert.NotNull(result.Data);
        Assert.Equal("MyPrinter", result.Data.Name);
    }

    [Fact]
    public async Task GetPlatform()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.System.Platform}")
            .Respond("application/json", "\"Linux\"");
        var result = await _service.GetPlatform();
        Assert.Equal("Linux", result.Data);
    }

    [Fact]
    public async Task GetHostname()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.System.Hostname}")
            .Respond("application/json", "\"ultimaker\"");
        var result = await _service.GetHostname();
        Assert.Equal("ultimaker", result.Data);
    }

    [Fact]
    public async Task GetFirmware()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.System.Firmware}")
            .Respond("application/json", "\"5.0.0\"");
        var result = await _service.GetFirmware();
        Assert.Equal("5.0.0", result.Data);
    }

    [Fact]
    public async Task GetFirmwareStatus()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.System.FirmwareStatus}")
            .Respond("application/json", "\"idle\"");
        var result = await _service.GetFirmwareStatus();
        Assert.Equal("idle", result.Data);
    }

    [Fact]
    public async Task GetFirmwareStable()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.System.FirmwareStable}")
            .Respond("application/json", "\"5.0.0\"");
        var result = await _service.GetFirmwareStable();
        Assert.Equal("5.0.0", result.Data);
    }

    [Fact]
    public async Task GetFirmwareLatest()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.System.FirmwareLatest}")
            .Respond("application/json", "\"5.1.0\"");
        var result = await _service.GetFirmwareLatest();
        Assert.Equal("5.1.0", result.Data);
    }

    [Fact]
    public async Task GetMemory()
    {
        var json = """{"total": 1000, "used": 500}""";
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.System.Memory}")
            .Respond("application/json", json);
        var result = await _service.GetMemory();
        Assert.NotNull(result.Data);
        Assert.Equal(1000, result.Data.Total);
    }

    [Fact]
    public async Task GetTime()
    {
        var json = """{"utc": 123456789}""";
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.System.Time}")
            .Respond("application/json", json);
        var result = await _service.GetTime();
        Assert.NotNull(result.Data);
        Assert.Equal(123456789, result.Data.UTC);
    }

    [Fact]
    public async Task GetLogs()
    {
        var json = """["log1", "log2"]""";
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.System.Logs}")
            .Respond("application/json", json);
        var result = await _service.GetLogs();
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Length);
    }

    [Fact]
    public async Task GetLogs_WithParams()
    {
        var json = """["log1"]""";
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.System.Logs}")
            .WithQueryString("boot", "1")
            .WithQueryString("lines", "10")
            .Respond("application/json", json);
        var result = await _service.GetLogs(1, 10);
        Assert.NotNull(result.Data);
        Assert.Single(result.Data);
    }

    [Fact]
    public async Task GetName()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.System.Name}")
            .Respond("application/json", "\"MyPrinter\"");
        var result = await _service.GetName();
        Assert.Equal("MyPrinter", result.Data);
    }

    [Fact]
    public async Task SetName_ReturnsSuccess()
    {
        var newName = "New Printer Name";
        _mockHttp.When(HttpMethod.Put, $"{BaseUrl}/{UltimakerPaths.System.Name}")
            .With(request =>
            {
                var content = request.Content.ReadAsStringAsync().Result;
                return content.Contains(newName); // It sends json string like "New Printer Name"
            })
            .Respond(HttpStatusCode.OK);

        var result = await _service.SetName(newName);

        Assert.Equal(HttpStatusCode.OK, result.Data);
    }

    [Fact]
    public async Task SetName_WithoutCredential_ThrowsException()
    {
        // Setup service without credentials
        var httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        var service = new SystemService(httpClient);

        await Assert.ThrowsAsync<Ultimaker.ApiClient.Core.Exceptions.MissingCredentialException>(() => service.SetName("test"));
    }

    [Fact]
    public async Task GetCountry()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.System.Country}")
            .Respond("application/json", "\"US\"");
        var result = await _service.GetCountry();
        Assert.Equal("US", result.Data);
    }

    [Fact]
    public async Task GetIsCountryLocked()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.System.IsCountryLocked}")
            .Respond("application/json", "true");
        var result = await _service.GetIsCountryLocked();
        Assert.True(result.Data);
    }

    [Fact]
    public async Task GetLanguage()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.System.Language}")
            .Respond("application/json", "\"en\"");
        var result = await _service.GetLanguage();
        Assert.Equal("en", result.Data);
    }

    [Fact]
    public async Task GetUptime()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.System.Uptime}")
            .Respond("application/json", "1000");
        var result = await _service.GetUptime();
        Assert.Equal(1000, result.Data);
    }

    [Fact]
    public async Task GetPrinterType()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.System.Type}")
            .Respond("application/json", "\"printer\"");
        var result = await _service.GetType();
        Assert.Equal("printer", result.Data);
    }

    [Fact]
    public async Task GetVariant()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.System.Variant}")
            .Respond("application/json", "\"Ultimaker S5\"");
        var result = await _service.GetVariant();
        Assert.Equal(PrinterVariant.S5, result.Data);
    }

    [Fact]
    public async Task GetHardware()
    {
        var json = """{"revision": 1, "typeid": 2}""";
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.System.Hardware}")
            .Respond("application/json", json);
        var result = await _service.GetHardware();
        Assert.NotNull(result.Data);
        Assert.Equal(1, result.Data.Revision);
    }

    [Fact]
    public async Task GetHardwareTypeId()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.System.HardwareTypeId}")
            .Respond("application/json", "2");
        var result = await _service.GetHardwareTypeId();
        Assert.Equal(2, result.Data);
    }

    [Fact]
    public async Task GetHardwareRevision()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.System.HardwareRevision}")
            .Respond("application/json", "1");
        var result = await _service.GetHardwareRevision();
        Assert.Equal(1, result.Data);
    }

    [Fact]
    public async Task GetId()
    {
        var guid = Guid.NewGuid();
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.System.Guid}")
            .Respond("application/json", $"\"{guid}\"");
        var result = await _service.GetId();
        Assert.Equal(guid, result.Data);
    }
}
