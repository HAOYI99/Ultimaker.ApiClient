using System.Net;
using System.Net.Mime;
using JetBrains.Annotations;
using RichardSzalay.MockHttp;
using Ultimaker.ApiClient.Core.Constants;
using Ultimaker.ApiClient.Core.Enums;
using Ultimaker.ApiClient.Core.Services;

namespace Ultimaker.ApiClient.Tests.Services;

[TestSubject(typeof(HistoryService))]
public class HistoryServiceTest
{
    private MockHttpMessageHandler _mockHttp;
    private readonly HistoryService _service;
    private const string BaseUrl = "http://localhost:8080";

    private const string ListHistoryPrintJobs =
        """
        [
          {
            "datetime_cleaned": null,
            "datetime_finished": null,
            "datetime_started": "2025-09-25T02:21:59",
            "extruders_used": {
              "0": false,
              "1": false
            },
            "interrupted_step": null,
            "material_0_guid": "c3211c12-c1b5-4ab0-9e1a-bac990ffd26a",
            "material_1_guid": "617ba9f1-1a4a-4ebb-9bc0-ba52b667ef97",
            "name": "test_jobname",
            "printcore_0_name": "AA 0.4",
            "printcore_0_revision": null,
            "printcore_1_name": "AA 0.4",
            "printcore_1_revision": null,
            "reprint_original_uuid": null,
            "result": null,
            "source": "WEB_API",
            "time_elapsed": 0,
            "time_estimated": 0,
            "time_total": 0,
            "uuid": "b2a309ce-692d-4bea-ad14-85aceacacf24"
          },
          {
            "datetime_cleaned": "2025-09-25T02:21:08",
            "datetime_finished": "2025-09-25T01:58:07",
            "datetime_started": "2025-09-24T01:28:18",
            "extruders_used": {
              "0": false,
              "1": false
            },
            "interrupted_step": null,
            "material_0_guid": "c3211c12-c1b5-4ab0-9e1a-bac990ffd26a",
            "material_1_guid": "617ba9f1-1a4a-4ebb-9bc0-ba52b667ef97",
            "name": "test_jobname",
            "printcore_0_name": "AA 0.4",
            "printcore_0_revision": null,
            "printcore_1_name": "AA 0.4",
            "printcore_1_revision": null,
            "reprint_original_uuid": null,
            "result": "Finished",
            "source": "WEB_API",
            "time_elapsed": 0,
            "time_estimated": 0,
            "time_total": 0,
            "uuid": "9e8d93aa-83f7-435d-80cd-4a1ef57503f7"
          }
        ]
        """;

    private const string CurrentPrintJob =
        """
        {
          "datetime_cleaned": null,
          "datetime_finished": null,
          "datetime_started": "2025-09-25T02:21:59",
          "extruders_used": {
            "0": false,
            "1": false
          },
          "interrupted_step": null,
          "material_0_guid": "c3211c12-c1b5-4ab0-9e1a-bac990ffd26a",
          "material_1_guid": "617ba9f1-1a4a-4ebb-9bc0-ba52b667ef97",
          "name": "test_jobname",
          "printcore_0_name": "AA 0.4",
          "printcore_0_revision": null,
          "printcore_1_name": "AA 0.4",
          "printcore_1_revision": null,
          "reprint_original_uuid": null,
          "result": null,
          "source": "WEB_API",
          "time_elapsed": 0,
          "time_estimated": 0,
          "time_total": 0,
          "uuid": "b2a309ce-692d-4bea-ad14-85aceacacf24"
        }
        """;

    private const string HistoryPrintJob =
        """
        {
          "datetime_cleaned": "2025-09-25T02:21:08",
          "datetime_finished": "2025-09-25T01:58:07",
          "datetime_started": "2025-09-24T01:28:18",
          "extruders_used": {
            "0": false,
            "1": false
          },
          "interrupted_step": null,
          "material_0_guid": "c3211c12-c1b5-4ab0-9e1a-bac990ffd26a",
          "material_1_guid": "617ba9f1-1a4a-4ebb-9bc0-ba52b667ef97",
          "name": "test_jobname",
          "printcore_0_name": "AA 0.4",
          "printcore_0_revision": null,
          "printcore_1_name": "AA 0.4",
          "printcore_1_revision": null,
          "reprint_original_uuid": null,
          "result": "Finished",
          "source": "WEB_API",
          "time_elapsed": 0,
          "time_estimated": 0,
          "time_total": 0,
          "uuid": "9e8d93aa-83f7-435d-80cd-4a1ef57503f7"
        }
        """;

    private const string ListOfHistoryEvents =
        """
        [
          {
            "message": "Print b2a309ce-692d-4bea-ad14-85aceacacf24 started with name test_jobname",
            "parameters": [
              "b2a309ce-692d-4bea-ad14-85aceacacf24",
              "test_jobname"
            ],
            "time": "2025-09-25T02:21:59",
            "type_id": 131072
          },
          {
            "message": "Print 9e8d93aa-83f7-435d-80cd-4a1ef57503f7 cleared",
            "parameters": [
              "9e8d93aa-83f7-435d-80cd-4a1ef57503f7"
            ],
            "time": "2025-09-25T02:21:08",
            "type_id": 131077
          }
        ]
        """;

    private const string HistoryEventsOfTypeId131072 =
        """
        [
            {
              "message": "Print b2a309ce-692d-4bea-ad14-85aceacacf24 started with name test_jobname",
              "parameters": [
                "b2a309ce-692d-4bea-ad14-85aceacacf24",
                "test_jobname"
              ],
              "time": "2025-09-25T02:21:59",
              "type_id": 131072
            }
        ]
        """;

    public HistoryServiceTest()
    {
        _mockHttp = new MockHttpMessageHandler();
        var httpClient = _mockHttp.ToHttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(0.5);
        httpClient.BaseAddress = new Uri(BaseUrl);
        _service = new HistoryService(httpClient);
    }

    [Fact]
    public async Task GetListOfHistoryJobs()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.History.PrintJobs}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                ListHistoryPrintJobs);
        var result = await _service.GetPrintJobs();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Length);
    }

    [Fact]
    public async Task GetHistoryJob_NotFinishJob()
    {
        var jobId = new Guid("b2a309ce-692d-4bea-ad14-85aceacacf24");
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.History.PrintJobIdPath(jobId)}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                CurrentPrintJob);
        var result = await _service.GetPrintJobById(jobId);
        Assert.NotNull(result);
        Assert.Equal(jobId, result.JobId);
        Assert.Equal("test_jobname", result.JobName);
        Assert.Equal(DateTimeKind.Utc, result.StartedDt.Kind);
        Assert.Null(result.CleanedDt);
        Assert.Null(result.FinishedDt);
        Assert.Equal("c3211c12-c1b5-4ab0-9e1a-bac990ffd26a", result.FirstMaterialGuid.ToString());
        Assert.Equal("617ba9f1-1a4a-4ebb-9bc0-ba52b667ef97", result.SecondMaterialGuid.ToString());
        Assert.Null(result.Result);
        Assert.Equal("WEB_API", result.Source);
    }

    [Fact]
    public async Task GetHistoryJob()
    {
        var jobId = new Guid("9e8d93aa-83f7-435d-80cd-4a1ef57503f7");
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.History.PrintJobIdPath(jobId)}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                HistoryPrintJob);
        var result = await _service.GetPrintJobById(jobId);
        Assert.NotNull(result);
        Assert.Equal(jobId, result.JobId);
        Assert.Equal("test_jobname", result.JobName);
        Assert.Equal(DateTimeKind.Utc, result.StartedDt.Kind);
        Assert.NotNull(result.CleanedDt);
        Assert.NotNull(result.FinishedDt);
        Assert.Equal(DateTimeKind.Utc, result.CleanedDt.Value.Kind);
        Assert.Equal(DateTimeKind.Utc, result.FinishedDt.Value.Kind);
        Assert.Equal("c3211c12-c1b5-4ab0-9e1a-bac990ffd26a", result.FirstMaterialGuid.ToString());
        Assert.Equal("617ba9f1-1a4a-4ebb-9bc0-ba52b667ef97", result.SecondMaterialGuid.ToString());
        Assert.Equal(JobResult.FINISHED, result.Result);
        Assert.Equal("WEB_API", result.Source);
    }

    [Fact]
    public async Task GetListOfHistoryEvents()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.History.Events}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                ListOfHistoryEvents);
        var result = await _service.GetEvents();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Length);
        Assert.Equal(DateTimeKind.Utc, result[0].Time.Kind);
        Assert.Equal(DateTimeKind.Utc, result[1].Time.Kind);
    }

    [Fact]
    public async Task GetListOfHistoryEvents_WithTypeId()
    {
        var typeId = 131072;
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.History.EventsQueryPath(null, null, typeId)}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                HistoryEventsOfTypeId131072);
        var result = await _service.GetEvents(null, null, typeId);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Equal(DateTimeKind.Utc, result[0].Time.Kind);
        Assert.Equal(typeId, result[0].TypeId);
    }
}