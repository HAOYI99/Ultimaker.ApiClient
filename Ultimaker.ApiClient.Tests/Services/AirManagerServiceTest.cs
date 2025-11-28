using System.Net;
using System.Net.Mime;
using JetBrains.Annotations;
using RichardSzalay.MockHttp;
using Ultimaker.ApiClient.Core.Constants;
using Ultimaker.ApiClient.Core.Enums;
using Ultimaker.ApiClient.Core.Services;

namespace Ultimaker.ApiClient.Tests.Services;

[TestSubject(typeof(AirManagerService))]
public class AirManagerServiceTest
{
    private MockHttpMessageHandler _mockHttp;
    private readonly AirManagerService _service;
    private const string BaseUrl = "http://localhost:8080";

    public AirManagerServiceTest()
    {
        _mockHttp = new MockHttpMessageHandler();
        var httpClient = _mockHttp.ToHttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(0.5);
        httpClient.BaseAddress = new Uri(BaseUrl);
        _service = new AirManagerService(httpClient);
    }

    [Fact]
    public async Task GetAirManager()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.AirManager.Base}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                {
                  "ambient_temperature": 33,
                  "exhaust_temperature": 36,
                  "fan_speed": 626,
                  "filter_age": 1051,
                  "filter_max_age": 1500,
                  "filter_status": "peak_performance",
                  "firmware_version": "1639647304",
                  "status": "available"
                }
                """);
        var result = await _service.Get();
        Assert.NotNull(result.Data);
        Assert.Equal(33, result.Data.AmbientTemperature);
        Assert.Equal(36, result.Data.ExhaustTemperature);
        Assert.Equal(626, result.Data.FanSpeed);
        Assert.Equal(1051, result.Data.FilterAge);
        Assert.Equal(1500, result.Data.FilterMaxAge);
        Assert.Equal(AirManagerFilterStatus.PEAK_PERFORMANCE, result.Data.FilterStatus);
        Assert.Equal("1639647304", result.Data.FirmwareVersion);
        Assert.Equal(AirManagerStatus.AVAILABLE, result.Data.Status);
    }
}