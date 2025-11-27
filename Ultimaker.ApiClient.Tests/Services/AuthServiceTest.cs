using System.Net;
using System.Net.Mime;
using JetBrains.Annotations;
using RichardSzalay.MockHttp;
using Ultimaker.ApiClient.Core.Constants;
using Ultimaker.ApiClient.Core.Dto.Request;
using Ultimaker.ApiClient.Core.Enums;
using Ultimaker.ApiClient.Core.Services;

namespace Ultimaker.ApiClient.Tests.Services;

[TestSubject(typeof(AuthService))]
public class AuthServiceTest
{
    private MockHttpMessageHandler _mockHttp;
    private readonly AuthService _service;
    private const string BaseUrl = "http://localhost:8080";

    public AuthServiceTest()
    {
        _mockHttp = new MockHttpMessageHandler();
        var httpClient = _mockHttp.ToHttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(0.5);
        httpClient.BaseAddress = new Uri(BaseUrl);
        _service = new AuthService(httpClient);
    }

    [Fact]
    public async Task RequestNewAuth()
    {
        var payload = new RequestAuthDto { Application = "app", User = "user" };
        _mockHttp
            .When(HttpMethod.Post, $"{BaseUrl}/{UltimakerPaths.Auth.Request}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                {
                  "id": "string",
                  "key": "string"
                }
                """);
        var result = await _service.Request(payload);
        Assert.NotNull(result);
        Assert.Equal("string", result.AuthId);
        Assert.Equal("string", result.AuthKey);
    }

    [Fact]
    public async Task CheckAuth()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.Auth.Check("authId")}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                {
                  "message": "unauthorized"
                }
                """);
        var result = await _service.Check("authId");
        Assert.NotNull(result);
        Assert.Equal(AuthStatus.UNAUTHORIZED, result.Status);
    }

    [Fact]
    public async Task VerifyAuth()
    {
        _mockHttp
            .When($"{BaseUrl}/{UltimakerPaths.Auth.Verify}")
            .Respond(HttpStatusCode.OK,
                MediaTypeNames.Application.Json,
                """
                {
                  "message": "ok"
                }
                """);
        var result = await _service.Verify();
        Assert.Equal(HttpStatusCode.OK, result);
    }
}