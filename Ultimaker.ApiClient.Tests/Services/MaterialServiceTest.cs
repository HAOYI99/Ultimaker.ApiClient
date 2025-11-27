using System.Net;
using System.Net.Mime;
using JetBrains.Annotations;
using RichardSzalay.MockHttp;
using Ultimaker.ApiClient.Core.Constants;
using Ultimaker.ApiClient.Core.Dto;
using Ultimaker.ApiClient.Core.Services;

namespace Ultimaker.ApiClient.Tests.Services;

[TestSubject(typeof(MaterialService))]
public class MaterialServiceTest
{
    private MockHttpMessageHandler _mockHttp;
    private readonly MaterialService _service;
    private readonly MaterialService _authedService;
    private const string BaseUrl = "http://localhost:8080";

    public MaterialServiceTest()
    {
        _mockHttp = new MockHttpMessageHandler();
        var httpClient = _mockHttp.ToHttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(0.5);
        httpClient.BaseAddress = new Uri(BaseUrl);
        _service = new MaterialService(httpClient);
        _authedService = new MaterialService(httpClient, new NetworkCredential("user", "pass"));
    }

    [Fact]
    public async Task GetMaterials()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.Material.Base}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                [
                  "<xml..../>"
                ]
                """);
        var result = await _service.GetAll();
        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetMaterialById()
    {
        var id = Guid.NewGuid();
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.Material.IdPath(id)}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                "<xml..../>"
                """);
        var result = await _service.GetById(id);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetMaterialById_NotFound()
    {
        var id = Guid.NewGuid();
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.Material.IdPath(id)}")
            .Respond(HttpStatusCode.NotFound,
                MediaTypeNames.Application.Json,
                """
                {
                  "message": "GUID not found"
                }
                """);
        var result = await _service.GetById(id);
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteMaterialById()
    {
        var id = Guid.NewGuid();
        _mockHttp
            .When(HttpMethod.Delete, $"{BaseUrl}/{UltimakerPaths.Material.IdPath(id)}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                {
                  "message": "Remove successful",
                  "result": true,
                }
                """);
        var result = await _authedService.DeleteById(id);
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotEmpty(result.Message);
    }

    [Fact]
    public async Task UploadMaterial()
    {
        _mockHttp
            .When(HttpMethod.Post, $"{BaseUrl}/{UltimakerPaths.Material.Base}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                {
                  "message": "Material profile stored",
                  "result": true,
                }
                """);
        var file = new FileItem([0x01, 0x02, 0x03], "test.xml.fdm_material");
        var result = await _authedService.Upload(file);
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotEmpty(result.Message);
    }
    
    [Fact]
    public async Task UploadMaterial_Conflict()
    {
        _mockHttp
            .When(HttpMethod.Post, $"{BaseUrl}/{UltimakerPaths.Material.Base}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                {
                  "message": "Material profile not stored",
                  "result": false,
                }
                """);
        var file = new FileItem([0x01, 0x02, 0x03], "test.xml.fdm_material");
        var result = await _authedService.Upload(file);
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.NotEmpty(result.Message);
    }
}