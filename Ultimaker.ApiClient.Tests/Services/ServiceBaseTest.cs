using System.Net;
using System.Net.Sockets;
using System.Text;
using JetBrains.Annotations;
using Ultimaker.ApiClient.Core;
using Ultimaker.ApiClient.Core.Services;

namespace Ultimaker.ApiClient.Tests.Services;

[TestSubject(typeof(ServiceBase))]
public class ServiceBaseTest
{
    private const string BaseUrl = "http://localhost:8080";

    [Fact]
    public async Task SendAsyncInternal_WhenHttpRequestExceptionWithSocketInner_ReturnsServiceUnavailable()
    {
        var service = new TestService(new HttpClient());
        var ex = new HttpRequestException(
            "No such host is known. (hostname.com:80)",
            new SocketException((int)SocketError.HostNotFound));

        var result = await service.SendInternal<string>(() => throw ex);

        Assert.False(result.Success);
        Assert.Equal((int)HttpStatusCode.ServiceUnavailable, result.StatusCode);
        Assert.Equal(
            $"{ex.Message}. It might be incorrect hostname, or the printer is completely offline.",
            result.Message);
        Assert.Same(ex, result.Exception);
    }

    [Fact]
    public async Task SendAsyncInternal_WhenTaskCanceledWithoutCancellationRequest_ReturnsRequestTimeout()
    {
        var service = new TestService(new HttpClient());
        var ex = new TaskCanceledException("Request timed out.");
        using var cts = new CancellationTokenSource();

        var result = await service.SendInternal<string>(() => throw ex, cts.Token);

        Assert.False(result.Success);
        Assert.Equal((int)HttpStatusCode.RequestTimeout, result.StatusCode);
        Assert.Equal(ex.Message, result.Message);
        Assert.Same(ex, result.Exception);
    }

    [Fact]
    public async Task SendAsyncInternal_WhenUnhandledException_ReturnsInternalServerError()
    {
        var service = new TestService(new HttpClient());
        var ex = new InvalidOperationException("Unexpected failure.");

        var result = await service.SendInternal<string>(() => throw ex);

        Assert.False(result.Success);
        Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.Equal(ex.Message, result.Message);
        Assert.Same(ex, result.Exception);
    }

    [Fact]
    public async Task PutAsync_SendsPutRequestWithPathContentAndToken()
    {
        var handler = new RecordingHandler((_, _) => Task.FromResult(
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("\"ok\"", Encoding.UTF8, "application/json")
            }));
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri(BaseUrl) };
        var service = new TestService(httpClient);
        var content = new StringContent("payload", Encoding.UTF8, "text/plain");
        using var cts = new CancellationTokenSource();

        var result = await service.Put<string>("api/test", content, cts.Token);

        Assert.True(result.Success);
        Assert.Equal("ok", result.Data);
        Assert.NotNull(handler.LastRequest);
        Assert.Equal(HttpMethod.Put, handler.LastRequest!.Method);
        Assert.Equal(new Uri($"{BaseUrl}/api/test"), handler.LastRequest.RequestUri);
        Assert.True(handler.LastCancellationToken.CanBeCanceled);
        Assert.Equal("payload", handler.LastRequestBody);
    }

    private sealed class TestService(HttpClient httpClient) : ServiceBase(httpClient)
    {
        public Task<UltimakerApiResponse<T>> SendInternal<T>(
            Func<Task<HttpResponseMessage>> sendAction,
            CancellationToken ct = default) => SendAsyncInternal<T>(sendAction, ct);

        public Task<UltimakerApiResponse<T?>> Put<T>(
            string path,
            HttpContent content,
            CancellationToken ct = default) => PutAsync<T>(path, content, ct);
    }

    private sealed class RecordingHandler(
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler) : HttpMessageHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }
        public CancellationToken LastCancellationToken { get; private set; }
        public string? LastRequestBody { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            LastRequest = request;
            LastCancellationToken = cancellationToken;
            LastRequestBody = request.Content == null
                ? null
                : await request.Content.ReadAsStringAsync(cancellationToken);

            return await handler(request, cancellationToken);
        }
    }
}
