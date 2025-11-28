using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using JetBrains.Annotations;
using RichardSzalay.MockHttp;
using Ultimaker.ApiClient.Core.Constants;
using Ultimaker.ApiClient.Core.Dto;
using Ultimaker.ApiClient.Core.Enums;
using Ultimaker.ApiClient.Core.Services;

namespace Ultimaker.ApiClient.Tests.Services;

[TestSubject(typeof(PrintJobService))]
public class PrintJobServiceTest
{
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly PrintJobService _service;
    private readonly PrintJobService _authedService;
    private const string BaseUrl = "http://localhost:8080";

    public PrintJobServiceTest()
    {
        _mockHttp = new MockHttpMessageHandler();
        var httpClient = _mockHttp.ToHttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(0.5);
        httpClient.BaseAddress = new Uri(BaseUrl);
        _service = new PrintJobService(httpClient);
        _authedService = new PrintJobService(httpClient, new NetworkCredential("user", "pass"));
    }

    [Fact]
    public async Task GetJob_NotFinish()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.PrintJob.Base}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                {
                  "datetime_cleaned": "",
                  "datetime_finished": "",
                  "datetime_started": "2025-09-25T02:21:59",
                  "name": "test_jobname",
                  "pause_source": "",
                  "progress": 0.99,
                  "reprint_original_uuid": "",
                  "result": "",
                  "source": "WEB_API",
                  "source_application": "abc",
                  "source_user": "abc",
                  "state": "printing",
                  "time_elapsed": 86697,
                  "time_total": 87665,
                  "uuid": "b2a309ce-692d-4bea-ad14-85aceacacf24"
                }
                """);
        var result = await _service.Get();
        Assert.NotNull(result.Data);
        Assert.Null(result.Data.CleanedDt);
        Assert.Null(result.Data.FinishedDt);
        Assert.Equal(DateTimeKind.Utc, result.Data.StartedDt.Kind);
        Assert.Equal("test_jobname", result.Data.JobName);
        Assert.Equal(0.99m, result.Data.Progress);
        Assert.Equal(JobResult.EMPTY, result.Data.Result);
        Assert.Equal(JobState.PRINTING, result.Data.State);
        Assert.Equal(86697, result.Data.TimeElapsed);
        Assert.Equal(87665, result.Data.TimeTotal);
        Assert.Equal("b2a309ce-692d-4bea-ad14-85aceacacf24", result.Data.JobId.ToString());
    }

    [Fact]
    public async Task GetJob_Finished()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.PrintJob.Base}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                {
                  "datetime_cleaned": "",
                  "datetime_finished": "2025-09-26T02:51:07",
                  "datetime_started": "2025-09-25T02:21:59",
                  "name": "test_jobname",
                  "pause_source": "",
                  "progress": 1,
                  "reprint_original_uuid": "",
                  "result": "Finished",
                  "source": "WEB_API",
                  "source_application": "abc",
                  "source_user": "abc",
                  "state": "post_print",
                  "time_elapsed": 87625,
                  "time_total": 87622,
                  "uuid": "b2a309ce-692d-4bea-ad14-85aceacacf24"
                }
                """);
        var result = await _service.Get();
        Assert.NotNull(result.Data);
        Assert.Null(result.Data.CleanedDt);
        Assert.NotNull(result.Data.FinishedDt);
        Assert.Equal(DateTimeKind.Utc, result.Data.FinishedDt.Value.Kind);
        Assert.Equal(DateTimeKind.Utc, result.Data.StartedDt.Kind);
        Assert.Equal("test_jobname", result.Data.JobName);
        Assert.Equal(1, result.Data.Progress);
        Assert.Equal(JobResult.FINISHED, result.Data.Result);
        Assert.Equal(JobState.POST_PRINT, result.Data.State);
        Assert.Equal(87625, result.Data.TimeElapsed);
        Assert.Equal(87622, result.Data.TimeTotal);
        Assert.Equal("b2a309ce-692d-4bea-ad14-85aceacacf24", result.Data.JobId.ToString());
    }

    [Fact]
    public async Task GetJob_NoJob()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.PrintJob.Base}")
            .Respond(HttpStatusCode.NoContent);
        var result = await _service.Get();
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task GetName()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.PrintJob.JobName}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "test_jobname"
                """);
        var result = await _service.GetName();
        Assert.Equal("test_jobname", result.Data);
    }

    [Fact]
    public async Task GetStartedDt()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.PrintJob.StartedDt}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "2025-09-25T02:21:59"
                """);
        var result = await _service.GetStartedDt();
        Assert.NotNull(result.Data);
        Assert.Equal(DateTimeKind.Utc, result.Data.Value.Kind);
    }

    [Fact]
    public async Task GetFinishedDt()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.PrintJob.FinishedDt}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "2025-09-25T02:21:59"
                """);
        var result = await _service.GetFinishedDt();
        Assert.NotNull(result.Data);
        Assert.Equal(DateTimeKind.Utc, result.Data.Value.Kind);
    }

    [Fact]
    public async Task GetCleanedDt()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.PrintJob.CleanedDt}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "2025-09-25T02:21:59"
                """);
        var result = await _service.GetCleanedDt();
        Assert.NotNull(result.Data);
        Assert.Equal(DateTimeKind.Utc, result.Data.Value.Kind);
    }

    [Fact]
    public async Task GetJobId()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.PrintJob.JobId}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "b2a309ce-692d-4bea-ad14-85aceacacf24"
                """);
        var result = await _service.GetJobId();
        Assert.NotNull(result.Data);
        Assert.Equal("b2a309ce-692d-4bea-ad14-85aceacacf24", result.Data.ToString());
    }

    [Fact]
    public async Task GetProgress()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.PrintJob.Progress}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "0.99"
                """);
        var result = await _service.GetProgress();
        Assert.NotNull(result.Data);
        Assert.Equal(0.99m, result.Data);
    }

    [Fact]
    public async Task GetJobState()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.PrintJob.State}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "wait_cleanup"
                """);
        var result = await _service.GetJobState();
        Assert.NotNull(result.Data);
        Assert.Equal(JobState.WAIT_CLEANUP, result.Data);
    }

    [Fact]
    public async Task GetJobResult()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.PrintJob.Result}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "Finished"
                """);
        var result = await _service.GetJobResult();
        Assert.NotNull(result.Data);
        Assert.Equal(JobResult.FINISHED, result.Data);
    }

    [Fact]
    public async Task GetTimeElapsed()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.PrintJob.TimeElapsed}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "86697"
                """);
        var result = await _service.GetTimeElapsed();
        Assert.NotNull(result.Data);
        Assert.Equal(86697, result.Data);
    }

    [Fact]
    public async Task GetTimeTotal()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.PrintJob.TimeTotal}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "87665"
                """);
        var result = await _service.GetTimeTotal();
        Assert.NotNull(result.Data);
        Assert.Equal(87665, result.Data);
    }

    [Fact]
    public async Task GetSource()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.PrintJob.Source}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "WEB_API"
                """);
        var result = await _service.GetSource();
        Assert.NotNull(result.Data);
        Assert.Equal("WEB_API", result.Data);
    }

    [Fact]
    public async Task GetSourceUser()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.PrintJob.SourceUser}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "abc"
                """);
        var result = await _service.GetSourceUser();
        Assert.NotNull(result.Data);
        Assert.Equal("abc", result.Data);
    }

    [Fact]
    public async Task GetSourceApp()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.PrintJob.SourceApp}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "abc"
                """);
        var result = await _service.GetSourceApp();
        Assert.NotNull(result.Data);
        Assert.Equal("abc", result.Data);
    }

    [Fact]
    public async Task GetPauseSrc()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.PrintJob.PauseSrc}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "test"
                """);
        var result = await _service.GetPauseSrc();
        Assert.NotNull(result.Data);
        Assert.Equal("test", result.Data);
    }

    [Fact]
    public async Task GetReprintOriginalUuid()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.PrintJob.ReprintOriginalUuid}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "b2a309ce-692d-4bea-ad14-85aceacacf24"
                """);
        var result = await _service.GetReprintOriginalUuid();
        Assert.NotNull(result.Data);
        Assert.Equal("b2a309ce-692d-4bea-ad14-85aceacacf24", result.Data.ToString());
    }

    [Fact]
    public async Task GetReprintOriginalUuid_NotReprint()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.PrintJob.ReprintOriginalUuid}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                ""
                """);
        var result = await _service.GetReprintOriginalUuid();
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task GetGCode()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.PrintJob.GCode}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Text.Plain,
                """
                ...long gcode...
                """);
        var result = await _authedService.GetGCode();
        Assert.NotNull(result.Data);
    }
    
    [Fact]
    public async Task GetGCode_NotFound()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.PrintJob.GCode}")
            .Respond(HttpStatusCode.NotFound,
                MediaTypeNames.Application.Json,
                """
                {"message" : "Not found"}
                """);
        var result = await _authedService.GetGCode();
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task GetContainer()
    {
        var fakeData = new byte[] { 1, 2, 3, 4, 5 };
        var content = new ByteArrayContent(fakeData);
        content.Headers.ContentDisposition =
            new ContentDispositionHeaderValue("attachment")
                { FileName = "test-jobname.ufp" };
        content.Headers.ContentType = new MediaTypeHeaderValue("application/x-ufp");
        _mockHttp
            .When( $"{BaseUrl}/{UltimakerPaths.PrintJob.Container}")
            .Respond(HttpStatusCode.OK, content);
        var result = await _authedService.GetContainer();
        Assert.NotNull(result.Data);
        Assert.Equal("test-jobname.ufp", result.Data.FileName);
        Assert.Equal(fakeData, result.Data.Content);
        Assert.Equal("test-jobname", result.Data.FileNameOnly);
    }
    
    [Fact]
    public async Task GetContainer_NotFound()
    {
        _mockHttp
            .When( $"{BaseUrl}/{UltimakerPaths.PrintJob.Container}")
            .Respond(HttpStatusCode.NotFound,
                MediaTypeNames.Application.Json,
                """
                {"message" : "Not found"}
                """);
        var result = await _authedService.GetContainer();
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task UpdateJobState()
    {
        _mockHttp
            .When(HttpMethod.Put, $"{BaseUrl}/{UltimakerPaths.PrintJob.State}")
            .Respond(HttpStatusCode.NoContent);
        var result = await _authedService.SetJobState(UpdateJobStateOpt.ABORT);
        Assert.Equal(HttpStatusCode.NoContent, result.Data);
    }

    [Fact]
    public async Task StartPrintJob()
    {
        var fakeData = new byte[] { 1, 2, 3, 4, 5 };
        _mockHttp
            .When(HttpMethod.Post, $"{BaseUrl}/{UltimakerPaths.PrintJob.Base}")
            .Respond(HttpStatusCode.Created,
                MediaTypeNames.Application.Json,
                """
                {
                  "uuid": "b2a309ce-692d-4bea-ad14-85aceacacf24",
                  "message":"Print job accepted"
                }
                """);
        var fileItem = new FileItem(fakeData, "test-jobname.ufp");
        var result = await _authedService.Start(fileItem);
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data.Message);
        Assert.Equal("b2a309ce-692d-4bea-ad14-85aceacacf24", result.Data.JobId.ToString());
    }
}