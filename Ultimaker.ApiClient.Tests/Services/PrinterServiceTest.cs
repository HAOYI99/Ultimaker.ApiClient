using System.Net;
using System.Net.Mime;
using JetBrains.Annotations;
using RichardSzalay.MockHttp;
using Ultimaker.ApiClient.Core.Constants;
using Ultimaker.ApiClient.Core.Dto.Response.Printer;
using Ultimaker.ApiClient.Core.Enums;
using Ultimaker.ApiClient.Core.Services;

namespace Ultimaker.ApiClient.Tests.Services;

[TestSubject(typeof(PrinterService))]
public class PrinterServiceTest
{
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly PrinterService _service;
    private const string BaseUrl = "http://localhost:8080";

    private const string FullPrinterResponse =
        """
        {
          "bed": {
            "pre_heat": {
              "active": false
            },
            "temperature": {
              "current": 95,
              "target": 95
            },
            "type": "flex"
          },
          "diagnostics": {},
          "heads": [
            {
              "acceleration": 2000,
              "extruders": [
                {
                  "active_material": {
                    "GUID": "2f9d2279-9b0e-4765-bf9b-d1e1e13f3c49",
                    "guid": "2f9d2279-9b0e-4765-bf9b-d1e1e13f3c49",
                    "length_remaining": -1
                  },
                  "feeder": {
                    "acceleration": 3000,
                    "jerk": 5,
                    "max_speed": 45
                  },
                  "hotend": {
                    "id": "AA+ 0.4",
                    "offset": {
                      "state": "valid",
                      "x": 22,
                      "y": 0,
                      "z": 0
                    },
                    "revision": "1",
                    "serial": "cf6d7e490000",
                    "statistics": {
                      "last_material_guid": "2f9d2279-9b0e-4765-bf9b-d1e1e13f3c49",
                      "material_extruded": 3284420,
                      "max_temperature_exposed": 261,
                      "prints_since_cleaned": "148",
                      "time_spent_hot": 3885540
                    },
                    "temperature": {
                      "current": 250.1,
                      "target": 250
                    }
                  }
                },
                {
                  "active_material": {
                    "GUID": "",
                    "guid": "",
                    "length_remaining": -1
                  },
                  "feeder": {
                    "acceleration": 3000,
                    "jerk": 5,
                    "max_speed": 45
                  },
                  "hotend": {
                    "id": "CC+ 0.4",
                    "offset": {
                      "state": "valid",
                      "x": 22.25609756097561,
                      "y": -0.10975609756097571,
                      "z": 0
                    },
                    "revision": "1",
                    "serial": "d214db4a0000",
                    "statistics": {
                      "last_material_guid": "0e01be8c-e425-4fb1-b4a3-b79f255f1db9",
                      "material_extruded": 3400,
                      "max_temperature_exposed": 220,
                      "prints_since_cleaned": "10",
                      "time_spent_hot": 425400
                    },
                    "temperature": {
                      "current": 69.4,
                      "target": 0
                    }
                  }
                }
              ],
              "fan": 5.019607843137255,
              "jerk": {
                "x": 20,
                "y": 20,
                "z": 0.4
              },
              "max_speed": {
                "x": 300,
                "y": 300,
                "z": 40
              },
              "position": {
                "x": 265.301,
                "y": 11.308,
                "z": 0.19999999999999996
              }
            }
          ],
          "led": {
            "blink": {},
            "brightness": 100,
            "hue": 0,
            "saturation": 0
          },
          "network": {
            "ethernet": {
              "connected": true,
              "enabled": true
            },
            "wifi": {
              "connected": false,
              "enabled": false,
              "mode": "CABLE",
              "ssid": "UM-NO-HOTSPOT-NAME-SET"
            },
            "wifi_networks": []
          },
          "status": "printing",
          "validate_header": {}
        }
        """;

    public PrinterServiceTest()
    {
        _mockHttp = new MockHttpMessageHandler();
        var httpClient = _mockHttp.ToHttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(0.5);
        httpClient.BaseAddress = new Uri(BaseUrl);
        _service = new PrinterService(httpClient);
    }

    [Fact]
    public async Task GetPrinter()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.Printer.Base}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                FullPrinterResponse);
        var result = await _service.Get();
        var printer = result.Data;
        Assert.NotNull(printer);
        var bedData = printer.Bed;
        Assert.False(bedData.PreHeat.Active);
        AssertTemperature(bedData.Temperature);
        Assert.Equal("flex", bedData.Type);
        var headData = printer.Heads;
        Assert.NotEmpty(headData);
        Assert.Single(headData);
        var firstHead = headData[0];
        Assert.True(firstHead.Fan > 0);
        Assert.Equal(2000, firstHead.Acceleration);
        AssertDimension(firstHead.Jerk);
        AssertDimension(firstHead.MaxSpeed);
        AssertDimension(firstHead.Position);
        Assert.NotEmpty(firstHead.Extruders);
        Assert.Equal(2, firstHead.Extruders.Length);
        var firstExtruder = firstHead.Extruders[0];
        var hotend = firstExtruder.Hotend;
        var offset = hotend.Offset;
        var statistics = hotend.Statistics;
        var feeder = firstExtruder.Feeder;
        var activeMaterial = firstExtruder.ActiveMaterial;
        Assert.Equal("AA+ 0.4", hotend.Id);
        Assert.Equal("cf6d7e490000", hotend.Serial);
        AssertTemperature(hotend.Temperature);
        Assert.Equal(Validity.VALID, offset.State);
        AssertDimension(offset);
        Assert.Equal("2f9d2279-9b0e-4765-bf9b-d1e1e13f3c49", statistics.LastMaterialGuid.ToString());
        Assert.True(statistics.MaterialExtruded >= 0);
        Assert.True(statistics.MaxTemperatureExposed >= 0);
        Assert.True(statistics.PrintsSinceCleaned >= 0);
        Assert.True(statistics.TimeSpentHot >= 0);
        Assert.Equal(3000, feeder.Acceleration);
        Assert.Equal(5, feeder.Jerk);
        Assert.Equal(45, feeder.MaxSpeed);
        Assert.Equal("2f9d2279-9b0e-4765-bf9b-d1e1e13f3c49", activeMaterial.Id.ToString());
        var led = printer.Led;
        Assert.NotNull(led);
        Assert.Equal(100, led.Brightness);
        Assert.Equal(0, led.Hue);
        Assert.Equal(0, led.Saturation);
    }

    [Fact]
    public async Task GetStatus()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.Status}")
            .Respond("application/json", "\"printing\"");
        var result = await _service.GetStatus();
        Assert.Equal(PrinterStatus.PRINTING, result.Data);
    }

    [Fact]
    public async Task GetLed()
    {
        var json = """{"brightness": 100, "hue": 0, "saturation": 0}""";
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.Led}")
            .Respond("application/json", json);
        var result = await _service.GetLed();
        Assert.NotNull(result.Data);
        Assert.Equal(100, result.Data.Brightness);
    }

    [Fact]
    public async Task GetLedHue()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.LedHue}")
            .Respond("application/json", "123.45");
        var result = await _service.GetLedHue();
        Assert.Equal(123.45m, result.Data);
    }

    [Fact]
    public async Task GetLedSaturation()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.LedSaturation}")
            .Respond("application/json", "50.0");
        var result = await _service.GetLedSaturation();
        Assert.Equal(50.0m, result.Data);
    }

    [Fact]
    public async Task GetLedBrightness()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.LedBrightness}")
            .Respond("application/json", "75.0");
        var result = await _service.GetLedBrightness();
        Assert.Equal(75.0m, result.Data);
    }

    [Fact]
    public async Task GetHeads()
    {
        // returns array of heads
        var json = """[{"acceleration": 2000, "extruders": []}]""";
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.Heads}")
            .Respond("application/json", json);
        var result = await _service.GetHeads();
        Assert.NotNull(result.Data);
        Assert.Single(result.Data);
        Assert.Equal(2000, result.Data[0].Acceleration);
    }

    [Fact]
    public async Task GetHead()
    {
        var json = """{"acceleration": 2000}""";
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.Head(0)}")
            .Respond("application/json", json);
        var result = await _service.GetHead(0);
        Assert.NotNull(result.Data);
        Assert.Equal(2000, result.Data.Acceleration);
    }

    [Fact]
    public async Task GetHeadPosition()
    {
        var json = """{"x": 10, "y": 20, "z": 30}""";
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.HeadPosition(0)}")
            .Respond("application/json", json);
        var result = await _service.GetHeadPosition(0);
        Assert.NotNull(result.Data);
        Assert.Equal(10, result.Data.X);
    }

    [Fact]
    public async Task GetHeadMaxSpeed()
    {
        var json = """{"x": 100, "y": 200, "z": 30}""";
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.HeadMaxSpeed(0)}")
            .Respond("application/json", json);
        var result = await _service.GetHeadMaxSpeed(0);
        Assert.NotNull(result.Data);
        Assert.Equal(100, result.Data.X);
    }

    [Fact]
    public async Task GetHeadJerk()
    {
        var json = """{"x": 5, "y": 6, "z": 7}""";
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.HeadJerk(0)}")
            .Respond("application/json", json);
        var result = await _service.GetHeadJerk(0);
        Assert.NotNull(result.Data);
        Assert.Equal(5, result.Data.X);
    }

    [Fact]
    public async Task GetHeadAcceleration()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.HeadAcceleration(0)}")
            .Respond("application/json", "3000");
        var result = await _service.GetHeadAcceleration(0);
        Assert.Equal(3000m, result.Data);
    }

    [Fact]
    public async Task GetBed()
    {
        var json = """{"type": "glass", "temperature": {"current": 60, "target": 60}}""";
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.Bed}")
            .Respond("application/json", json);
        var result = await _service.GetBed();
        Assert.NotNull(result.Data);
        Assert.Equal("glass", result.Data.Type);
    }

    [Fact]
    public async Task GetBedTemperature()
    {
        var json = """{"current": 60, "target": 60}""";
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.BedTemperature}")
            .Respond("application/json", json);
        var result = await _service.GetBedTemperature();
        Assert.NotNull(result.Data);
        Assert.Equal(60, result.Data.Current);
    }

    [Fact]
    public async Task GetBedPreHeat()
    {
        var json = """{"active": true}""";
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.BedPreHeat}")
            .Respond("application/json", json);
        var result = await _service.GetBedPreHeat();
        Assert.NotNull(result.Data);
        Assert.True(result.Data.Active);
    }

    [Fact]
    public async Task GetBedType()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.BedType}")
            .Respond("application/json", "\"glass\"");
        var result = await _service.GetBedType();
        Assert.Equal("glass", result.Data);
    }

    [Fact]
    public async Task GetExtruders()
    {
        var json = """[{"feeder": {"max_speed": 50}}]""";
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.Extruders(0)}")
            .Respond("application/json", json);
        var result = await _service.GetExtruders(0);
        Assert.NotNull(result.Data);
        Assert.Single(result.Data);
    }

    [Fact]
    public async Task GetExtruder()
    {
        var json = """{"feeder": {"max_speed": 50}}""";
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.Extruder(0, 0)}")
            .Respond("application/json", json);
        var result = await _service.GetExtruder(0, 0);
        Assert.NotNull(result.Data);
        Assert.Equal(50, result.Data.Feeder.MaxSpeed);
    }

    [Fact]
    public async Task GetActiveMaterial()
    {
        var guid = Guid.NewGuid();
        var json = $"{{\"guid\": \"{guid}\"}}";
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.ActiveMaterial(0, 0)}")
            .Respond("application/json", json);
        var result = await _service.GetActiveMaterial(0, 0);
        Assert.NotNull(result.Data);
        Assert.Equal(guid, result.Data.Id);
    }

    [Fact]
    public async Task GetActiveMaterialGuid()
    {
        var guid = Guid.NewGuid();
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.ActiveMaterialId(0, 0)}")
            .Respond("application/json", $"\"{guid}\"");
        var result = await _service.GetActiveMaterialGuid(0, 0);
        Assert.Equal(guid, result.Data);
    }

    [Fact]
    public async Task GetFeeder()
    {
        var json = """{"max_speed": 60}""";
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.Feeder(0, 0)}")
            .Respond("application/json", json);
        var result = await _service.GetFeeder(0, 0);
        Assert.NotNull(result.Data);
        Assert.Equal(60, result.Data.MaxSpeed);
    }

    [Fact]
    public async Task GetFeederJerk()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.FeederJerk(0, 0)}")
            .Respond("application/json", "10");
        var result = await _service.GetFeederJerk(0, 0);
        Assert.Equal(10m, result.Data);
    }

    [Fact]
    public async Task GetFeederMaxSpeed()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.FeederMaxSpeed(0, 0)}")
            .Respond("application/json", "60");
        var result = await _service.GetFeederMaxSpeed(0, 0);
        Assert.Equal(60m, result.Data);
    }

    [Fact]
    public async Task GetFeederAcceleration()
    {
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.FeederAcceleration(0, 0)}")
            .Respond("application/json", "500");
        var result = await _service.GetFeederAcceleration(0, 0);
        Assert.Equal(500m, result.Data);
    }

    [Fact]
    public async Task GetHotend()
    {
        var json = """{"id": "AA 0.4", "temperature": {"current": 200}}""";
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.Hotend(0, 0)}")
            .Respond("application/json", json);
        var result = await _service.GetHotend(0, 0);
        Assert.NotNull(result.Data);
        Assert.Equal("AA 0.4", result.Data.Id);
    }

    [Fact]
    public async Task GetHotendOffset()
    {
        var json = """{"state": "valid", "x": 1, "y": 2, "z": 3}""";
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.HotendOffset(0, 0)}")
            .Respond("application/json", json);
        var result = await _service.GetHotendOffset(0, 0);
        Assert.NotNull(result.Data);
        Assert.Equal(Validity.VALID, result.Data.State);
    }

    [Fact]
    public async Task GetHotendTemperature()
    {
        var json = """{"current": 210, "target": 210}""";
        _mockHttp.When($"{BaseUrl}/{UltimakerPaths.Printer.HotendTemperature(0, 0)}")
            .Respond("application/json", json);
        var result = await _service.GetHotendTemperature(0, 0);
        Assert.NotNull(result.Data);
        Assert.Equal(210, result.Data.Current);
    }

    private static void AssertDimension(DimensionDto dimension)
    {
        Assert.NotNull(dimension);
        Assert.True(dimension.X >= 0);
        Assert.True(dimension.Y >= 0);
        Assert.True(dimension.Z >= 0);
    }

    private static void AssertTemperature(TemperatureDto temperature)
    {
        Assert.NotNull(temperature);
        Assert.True(temperature.Current >= 0);
        Assert.True(temperature.Target >= 0);
    }
}
