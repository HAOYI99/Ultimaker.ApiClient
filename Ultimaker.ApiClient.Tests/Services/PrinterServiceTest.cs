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